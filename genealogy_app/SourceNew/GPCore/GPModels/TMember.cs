﻿using System;
using System.Collections.Generic;

namespace GPModels
{
    public class TMember : BaseModel
    {
        public string Name { get; set; }
        public string ShowName { get; set; }
        public Dictionary<string, string> TypeName { get; set; } = new Dictionary<string, string>();
        public int Gender { get; set; }
        public int ChildLevelInFamily { get; set; }
        public string National { get; set; }
        public string Religion { get; set; }
        public string BirthPlace { get; set; }
        public bool IsDeath { get; set; }
        public VNDate Birthday { get; set; }
        public bool IsLeapMonthDB { get; set; }
        public VNDate DeadDay { get; set; }
        public bool IsLeapMonthDD { get; set; }
        public string DeadPlace { get; set; }
        public string HomeTown { get; set; }
        public int LevelInFamily { get; set; }
        public string LevelInFamilyOfFather { get; set; }
        public string LevelInFamilyOfMother { get; set; }
        public List<TMemberRelation> Relation { get; set; }
        public List<string> ListPARENT { get; set; }
        public List<string> ListSPOUSE { get; set; }
        public List<string> ListCHILDREN { get; set; }
        public MemberContact Contact { get; set; }
        public List<TMemberJob> Job { get; set; }
        public List<TMemberSchool> School { get; set; }
        public List<TMemberEvent> Event { get; set; }
        public string AvatarImg { get; set; }
        public string strBackColor { get; set; }
        public string strForeColor { get; set; }
        public bool UseDefaultColor { get; set; } = true;
        public bool ShowStepChild { get; set; } = false;
        public bool InRootTree { get; set; } = false;
        public bool SpouseInRootTree { get; set; } = false;
        public string RootID { get; set; }

        public TMember()
        {
            Birthday = new VNDate();
            DeadDay = new VNDate();
            Contact = new MemberContact();
            Relation = new List<TMemberRelation>();
            ListPARENT = new List<string>();
            ListSPOUSE = new List<string>();
            ListCHILDREN = new List<string>();
            Job = new List<TMemberJob>();
            School = new List<TMemberSchool>();
            Event = new List<TMemberEvent>();
        }
    }
}
