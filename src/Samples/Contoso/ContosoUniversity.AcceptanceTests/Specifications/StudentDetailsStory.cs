﻿using Specify.Stories;

namespace ContosoUniversity.AcceptanceTests.Specifications
{
    public class StudentDetailsStory : UserStory
    {
        public StudentDetailsStory()
        {
            AsA = "Student Administrator";
            IWant = "View the details of a Student";
            SoThat = "I can see what grade she achieved";
        }
    }
}