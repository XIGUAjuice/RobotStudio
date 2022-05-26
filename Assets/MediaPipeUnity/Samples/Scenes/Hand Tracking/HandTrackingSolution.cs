// Copyright (c) 2021 homuler
//
// Use of this source code is governed by an MIT-style
// license that can be found in the LICENSE file or at
// https://opensource.org/licenses/MIT.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mediapipe.Unity.HandTracking
{
    public class HandTrackingSolution : ImageSourceSolution<HandTrackingGraph>
    {
        public GameObject objectEndEffector;
        private EndEffector endEffector;        // 末端执行器类对象
        private float lastDistance = -1;
        private float threshold = 0.01f;


        [SerializeField] private DetectionListAnnotationController _palmDetectionsAnnotationController;
        [SerializeField] private NormalizedRectListAnnotationController _handRectsFromPalmDetectionsAnnotationController;
        [SerializeField] private MultiHandLandmarkListAnnotationController _handLandmarksAnnotationController;
        [SerializeField] private NormalizedRectListAnnotationController _handRectsFromLandmarksAnnotationController;
        private float lastX = 0;
        private float lastY = 0;
        private float lastZ = 0;

        public HandTrackingGraph.ModelComplexity modelComplexity
        {
            get => graphRunner.modelComplexity;
            set => graphRunner.modelComplexity = value;
        }

        public int maxNumHands
        {
            get => graphRunner.maxNumHands;
            set => graphRunner.maxNumHands = value;
        }

        public float minDetectionConfidence
        {
            get => graphRunner.minDetectionConfidence;
            set => graphRunner.minDetectionConfidence = value;
        }

        public float minTrackingConfidence
        {
            get => graphRunner.minTrackingConfidence;
            set => graphRunner.minTrackingConfidence = value;
        }

        protected override void OnStartRun()
        {
            endEffector = objectEndEffector.GetComponent<EndEffector>();        // 获取末端执行器类对象

            if (!runningMode.IsSynchronous())
            {
                graphRunner.OnPalmDetectectionsOutput += OnPalmDetectionsOutput;
                graphRunner.OnHandRectsFromPalmDetectionsOutput += OnHandRectsFromPalmDetectionsOutput;
                graphRunner.OnHandLandmarksOutput += OnHandLandmarksOutput;
                // TODO: render HandWorldLandmarks annotations
                graphRunner.OnHandRectsFromLandmarksOutput += OnHandRectsFromLandmarksOutput;
                graphRunner.OnHandednessOutput += OnHandednessOutput;
            }

            var imageSource = ImageSourceProvider.ImageSource;
            SetupAnnotationController(_palmDetectionsAnnotationController, imageSource, true);
            SetupAnnotationController(_handRectsFromPalmDetectionsAnnotationController, imageSource, true);
            SetupAnnotationController(_handLandmarksAnnotationController, imageSource, true);
            SetupAnnotationController(_handRectsFromLandmarksAnnotationController, imageSource, true);
        }

        protected override void AddTextureFrameToInputStream(TextureFrame textureFrame)
        {
            graphRunner.AddTextureFrameToInputStream(textureFrame);
        }

        private float squareEuclidean(NormalizedLandmark p1, NormalizedLandmark p2)
        {
            return Mathf.Pow(p1.X - p2.X, 2) + Mathf.Pow(p1.Y - p2.Y, 2) + Mathf.Pow(p1.Z - p2.Z, 2);
        }

        private int fingerNumRecognizer(float thresDistance, float indexDistance, float middleDistance, float ringDistance, float pinkyDistance)
        {
            if (thresDistance < indexDistance && thresDistance > middleDistance && thresDistance > ringDistance && thresDistance > pinkyDistance)
            {
                return 1;
            }
            else if (thresDistance < indexDistance && thresDistance < middleDistance && thresDistance > ringDistance && thresDistance > pinkyDistance)
            {
                return 2;
            }
            else if (thresDistance > indexDistance && thresDistance < middleDistance && thresDistance < ringDistance && thresDistance < pinkyDistance)
            {
                return 3;
            }
            else
            {
                return -1;
            }
        }

        protected override IEnumerator WaitForNextValue()
        {
            List<Detection> palmDetections = null;
            List<NormalizedRect> handRectsFromPalmDetections = null;
            List<NormalizedLandmarkList> handLandmarks = null;
            List<LandmarkList> handWorldLandmarks = null;
            List<NormalizedRect> handRectsFromLandmarks = null;
            List<ClassificationList> handedness = null;

            if (runningMode == RunningMode.Sync)
            {
                var _ = graphRunner.TryGetNext(out palmDetections, out handRectsFromPalmDetections, out handLandmarks, out handWorldLandmarks, out handRectsFromLandmarks, out handedness, true);
            }
            else if (runningMode == RunningMode.NonBlockingSync)
            {
                yield return new WaitUntil(() => graphRunner.TryGetNext(out palmDetections, out handRectsFromPalmDetections, out handLandmarks, out handWorldLandmarks, out handRectsFromLandmarks, out handedness, false));
            }

            // _palmDetectionsAnnotationController.DrawNow(palmDetections);
            // _handRectsFromPalmDetectionsAnnotationController.DrawNow(handRectsFromPalmDetections);
            _handLandmarksAnnotationController.DrawNow(handLandmarks, handedness);
            // TODO: render HandWorldLandmarks annotations
            // _handRectsFromLandmarksAnnotationController.DrawNow(handRectsFromLandmarks);
            if (handLandmarks != null)
            {
                if (handLandmarks.Count == 1)
                {
                    lastDistance = -1;

                    float indexFingerX = handLandmarks[0].Landmark[8].X;
                    float indexFingerY = handLandmarks[0].Landmark[8].Y;
                    float indexFingerZ = handLandmarks[0].Landmark[8].Z;
                    if (lastZ != 0)
                    {
                        float deltaX = (indexFingerX - lastX) * 100;
                        float deltaY = (indexFingerY - lastY) * 100;
                        float deltaZ = (indexFingerZ - lastZ) * 200;
                        if (deltaX < 1 && deltaX > -1)
                        {
                            deltaX = 0;
                        }
                        if (deltaY < 1 && deltaY > -1)
                        {
                            deltaY = 0;
                        }
                        if (deltaZ < 1 && deltaZ > -1)
                        {
                            deltaZ = 0;
                        }
                        endEffector.moveInCameraTrans(new Vector3(deltaX, -deltaY, -deltaZ), new Vector3(0, 0, 0));
                    }
                    lastX = indexFingerX;
                    lastY = indexFingerY;
                    lastZ = indexFingerZ;
                }
                else if (handLandmarks.Count == 2)
                {   
                    int leftHand = 0;
                    int rightHand = 0;
                    if(handedness[0].Classification[0].Label.Equals("Left"))
                    {
                        leftHand = 0;
                        rightHand = 1;
                    }
                    else
                    {
                        leftHand = 1;
                        rightHand = 0;
                    }
                    // 阈值距离(中指第三关节到手腕的距离)
                    float thresDistance = squareEuclidean(handLandmarks[leftHand].Landmark[0], handLandmarks[leftHand].Landmark[10]);
                    // 食指到手腕的距离
                    float indexDistance = squareEuclidean(handLandmarks[leftHand].Landmark[0], handLandmarks[leftHand].Landmark[8]);
                    // 中指到手腕的距离
                    float middleDistance = squareEuclidean(handLandmarks[leftHand].Landmark[0], handLandmarks[leftHand].Landmark[12]);
                    // 无名指到手腕的距离
                    float ringDistance = squareEuclidean(handLandmarks[leftHand].Landmark[0], handLandmarks[leftHand].Landmark[16]);
                    // 小拇指到手腕的距离
                    float pinkyDistance = squareEuclidean(handLandmarks[leftHand].Landmark[0], handLandmarks[leftHand].Landmark[20]);

                    int number = fingerNumRecognizer(thresDistance, indexDistance, middleDistance, ringDistance, pinkyDistance);

                    float distance = Mathf.Sqrt(squareEuclidean(handLandmarks[rightHand].Landmark[4], handLandmarks[rightHand].Landmark[8]));
                    float degree = 0;
                    
                    if(lastDistance == -1)
                    {
                        lastDistance = distance;
                    }
                    else if (distance - lastDistance > threshold)
                    {
                        degree = (distance - lastDistance) * 180;
                    }
                    else if (distance - lastDistance < -threshold)
                    {
                        degree = (distance - lastDistance) * 180;
                    }

                    if (degree != 0)
                    {
                        if (number == 1)
                        {   
                            // 绕自身的x轴转
                            endEffector.moveInLocalTrans(new Vector3(0, 0, 0), new Vector3(degree, 0, 0));
                        }
                        else if (number == 2)
                        {
                            // 绕自身的y轴转
                            endEffector.moveInLocalTrans(new Vector3(0, 0, 0), new Vector3(0, degree, 0));
                        }
                        else if (number == 3)
                        {
                            // 绕自身的z轴转
                            endEffector.moveInLocalTrans(new Vector3(0, 0, 0), new Vector3(0, 0, degree));
                        }
                        lastDistance = distance;
                    }
                }
                else
                {
                    lastDistance = -1;
                    lastX = 0;
                    lastY = 0;
                    lastZ = 0;
                }
            }
        }

        private void OnPalmDetectionsOutput(object stream, OutputEventArgs<List<Detection>> eventArgs)
        {
            _palmDetectionsAnnotationController.DrawLater(eventArgs.value);
        }

        private void OnHandRectsFromPalmDetectionsOutput(object stream, OutputEventArgs<List<NormalizedRect>> eventArgs)
        {
            _handRectsFromPalmDetectionsAnnotationController.DrawLater(eventArgs.value);
        }

        private void OnHandLandmarksOutput(object stream, OutputEventArgs<List<NormalizedLandmarkList>> eventArgs)
        {
            _handLandmarksAnnotationController.DrawLater(eventArgs.value);
        }

        private void OnHandRectsFromLandmarksOutput(object stream, OutputEventArgs<List<NormalizedRect>> eventArgs)
        {
            _handRectsFromLandmarksAnnotationController.DrawLater(eventArgs.value);
        }

        private void OnHandednessOutput(object stream, OutputEventArgs<List<ClassificationList>> eventArgs)
        {
            _handLandmarksAnnotationController.DrawLater(eventArgs.value);
        }
    }
}
