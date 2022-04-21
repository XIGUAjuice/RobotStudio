/* AIUI返回的json对象的实体类 */
/* 通过 https://json2csharp.com/ 自动生成 */

using System.Collections.Generic;
using System;

    [Serializable]
    public class Slot
    {
        public int begin;
        public int end;
        public string name;
        public string normValue;
        public string value;
    }

    [Serializable]
    public class Semantic
    {
        public string entrypoint;
        public bool hazard;
        public string intent;
        public int score;
        public List<Slot> slots;
        public string template;
    }

    [Serializable]
    public class Intent
    {
        public string category;
        public string intentType;
        public int rc;
        public List<Semantic> semantic;
        public int semanticType;
        public string service;
        public string sid;
        public object state;
        public string text;
        public string uuid;
        public string vendor;
        public string version;
    }

    [Serializable]
    public class Data
    {
        public string sub;
        public string auth_id;
        public Intent intent;
        public int result_id;
        public bool is_last;
        public bool is_finish;
    }

    [Serializable]
    public class Root
    {
        public string action;
        public Data data;
        public string sid;
        public string code;
        public string desc;
    }