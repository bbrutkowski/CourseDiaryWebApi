﻿using CourseDiary.Domain.Interfaces;
using CourseDiary.Domain.Models;
using System.Threading.Tasks;
using System;

namespace CourseDiary.Domain
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;

        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<bool> Add(Course course)
        {
            if(course.Students.Count < 5 || course.Students.Count > 20)
            {
                throw new Exception("The amount of students must be between 5 and 20");
            }
            else if(course.Trainer == null)
            {
                throw new Exception("Trainer must be assaigned");
            }
            DateTime dt = DateTime.ParseExact(course.BeginDate.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            course.BeginDate = dt;
            return await _courseRepository.Add(course);
        }
    }
}
