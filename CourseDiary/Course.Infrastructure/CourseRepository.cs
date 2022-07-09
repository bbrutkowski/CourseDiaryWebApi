﻿using CourseDiary.Domain.Interfaces;
using System.Configuration;
using System.Threading.Tasks;
using CourseDiary.Domain.Models;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;

namespace CourseDiary.Infrastructure
{   

    public class CourseRepository : ICourseRepository
    {
        private string _connectionString = ConfigurationManager.ConnectionStrings["CourseDiaryDBConnectionString"].ConnectionString;

        public async Task<bool> Add(Course course)
        {
            bool success;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string commandText = $"INSERT INTO [Courses] ([Name],[BeginDate],[TrainerId],[PresenceTreshold],[HomeworkTreshold],[TestTreshold],[State]) VALUES (@Name, @BeginDate, @TrainerId, @PresenceTreshold, @HomeworkTreshold, @TestTreshold, @State)";
                    SqlCommand command = new SqlCommand(commandText, connection);
                    command.Parameters.Add("@Name", SqlDbType.VarChar, 255).Value = course.Name;
                    command.Parameters.Add("@BeginDate", SqlDbType.DateTime2).Value = course.BeginDate;
                    command.Parameters.Add("@TrainerId", SqlDbType.Int).Value = course.Trainer;
                    command.Parameters.Add("@PresenceTreshold", SqlDbType.Float, 8).Value = course.PresenceTreshold;
                    command.Parameters.Add("@HomeworkTreshold", SqlDbType.Float, 8).Value = course.HomeworkTreshold;
                    command.Parameters.Add("@TestTreshold", SqlDbType.Float, 8).Value = course.TestTreshold;
                    command.Parameters.Add("@State", SqlDbType.VarChar, 255).Value = course.State;
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    success = rowsAffected == 1;

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success = false;
            }

            return success;
        }

        public async Task<bool> ClosingCourse(Course course)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string commandSql = "UPDATE Courses SET State = @State WHERE Name = @Name";
                    SqlCommand command = new SqlCommand(commandSql, connection);
                    command.Parameters.Add("@Name", SqlDbType.VarChar, 255).Value = course.Name;
                    command.Parameters.Add("@State", SqlDbType.VarChar, 255).Value = course.State;

                    if (command.ExecuteNonQuery() == 1)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }


        public async Task<List<Course>> GetAllCoursesAsync()
        {
            List<Course> courses = new List<Course>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string commandText = @"SELECT * FROM [Courses]";
                    SqlCommand command = new SqlCommand(commandText, connection);
                    SqlDataReader dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        Course course = new Course();
                        course.Id = int.Parse(dataReader["Id"].ToString());
                        course.Name = dataReader["Name"].ToString();
                        course.BeginDate = DateTime.Parse(dataReader["BeginDate"].ToString());
                        course.Trainer = new Trainer
                        {
                            Id = int.Parse(dataReader["Id"].ToString()),
                            Name = dataReader["Name"].ToString(),
                            Surname = dataReader["Surname"].ToString(),
                            Email = dataReader["Email"].ToString(),
                            Password = dataReader["Password"].ToString(),
                            DateOfBirth = DateTime.Parse(dataReader["DateOfBirth"].ToString())
                        };
                        course.PresenceTreshold = double.Parse(dataReader["TestTreshold"].ToString());
                        course.HomeworkTreshold = double.Parse(dataReader["TestTreshold"].ToString());
                        course.TestTreshold = double.Parse(dataReader["TestTreshold"].ToString());
                        course.State = Enum.TryParse(dataReader["State"].ToString(), out State cos) ? cos : course.State;

                        courses.Add(course);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                courses = null;
            }

            return courses;
        }

        public async Task<bool> AddHomeworkResult(HomeworkResults result)
        {
            bool success;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string commandText = $"INSERT INTO [HomeworkResults] ([HomeWorkName],[FinishDate],[StudentId],[Results] VALUES (@HomeWorkName, @FinishDate, @StudentId, @Results)";
                    SqlCommand command = new SqlCommand(commandText, connection);
                    command.Parameters.Add("@HomeWorkName", SqlDbType.VarChar, 255).Value = result.HomeworkName;
                    command.Parameters.Add("@FinishDate", SqlDbType.Date).Value = result.FinishDate;
                    command.Parameters.Add("@StudentId", SqlDbType.Int).Value = result.Id;
                    command.Parameters.Add("@Results", SqlDbType.Float, 8).Value = result.Result;
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    success = rowsAffected == 1;

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success = false;
            }

            return success;
        }


        public async Task<bool> AddPresence(StudentPresence presence)
        {
            bool success;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string commandText = "INSERT INTO [StudentPresence] ([LessonDate],[CourseId],[StudentId],[Presence]) VALUES (@LessonDate, @CourseId, @StudentId, @Presence)";
                    SqlCommand command = new SqlCommand(commandText, connection);
                    command.Parameters.Add("@LessonDate", SqlDbType.Date).Value = presence.LessonDate;
                    command.Parameters.Add("@CourseId", SqlDbType.Int).Value = presence.Course.Id;
                    command.Parameters.Add("@StudentId", SqlDbType.Int).Value = presence.Student.Id;
                    command.Parameters.Add("@Presence", SqlDbType.NVarChar, 255).Value = presence.Presence;

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    success = rowsAffected == 1;

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success = false;
            }

            return success;
        }

        public async Task<bool> AddTestResult(TestResults testResult)
        {
            bool success;

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string commandText = $"INSERT INTO [TestResults] ([TestName],[FinishDate],[StudentId],[Results] VALUES (@TestName, @FinishDate, @StudentId, @Results)";
                    SqlCommand command = new SqlCommand(commandText, connection);
                    command.Parameters.Add("@TestName", SqlDbType.VarChar, 255).Value = testResult.TestName;
                    command.Parameters.Add("@FinishDate", SqlDbType.Date).Value = testResult.FinishDate;
                    command.Parameters.Add("@StudentId", SqlDbType.Int).Value = testResult.Id;
                    command.Parameters.Add("@Results", SqlDbType.Float, 8).Value = testResult.Result;
                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    success = rowsAffected == 1;

                    connection.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success = false;
            }

            return success;
        }

        public async Task<List<CourseResult>> GetAllCourseResults(int id)
        {
            List<CourseResult> courseResults = new List<CourseResult>();

            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    await connection.OpenAsync();

                    string commandText = @"SELECT
	                                     [CourseStudents].[Id] AS [ID],
	                                     [CourseStudents].[CourseId],
	                                     [HomeworkResults].[StudentId],
	                                     [HomeworkResults].[HomeWorkName],	
	                                     [HomeworkResults].[Results],
	                                     [TestResults].[TestName],
	                                     [TestResults].[Results],
	                                     [Students].[Email]
	                                     FROM[CourseStudents], [HomeworkResults], [TestResults], [Students]
	                                     WHERE CourseId = '@CourseId'";
                    SqlCommand command = new SqlCommand(commandText, connection);
                    SqlDataReader dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        CourseResult result = new CourseResult();
                        result.HomeworkResults = new HomeworkResults()
                        {
                            StudentId = int.Parse(dataReader["StudentId"].ToString()),
                            Result = float.Parse(dataReader["Result"].ToString()),
                        };
                        result.TestResults = new TestResults()
                        {
                            StudentId = int.Parse(dataReader["StudentId"].ToString()),
                            Result = float.Parse(dataReader["Result"].ToString()),
                        };
                        result.StudentPresence.Student.Email = dataReader["Email"].ToString();                      

                        courseResults.Add(result);
                    }                
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                courseResults = null;
            }

            return courseResults;
        }
    }
}
