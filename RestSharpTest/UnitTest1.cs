using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace RestSharpTest
{
    public class EmployeePayroll
    {
        public int id { get; set; }

        public string name { get; set; }

        public string salary { get; set; }

    }

    [TestClass]
    public class UnitTest1
    {
        RestClient restClient;

        /// <summary>
        /// Creates Client and by passing root URL, establishes connection with the server
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            restClient = new RestClient("http://localhost:4000");
        }

        /// <summary>
        /// TC1 Retrieves All employee details by GET method
        /// </summary>
        [TestMethod]
        public void RetrieveEmployeeData()
        {
            IRestResponse response = GetEmployeeDetails();

            //Checks whether the data is retrieved or not(200 is statuscode for retrieval)
            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            List<EmployeePayroll> records = JsonConvert.DeserializeObject<List<EmployeePayroll>>(response.Content);

            Assert.AreEqual(11, records.Count);

            foreach (EmployeePayroll employee in records)
            {
                Console.WriteLine("EmployeeID: "+employee.id+", EmployeeName: "+employee.name+", Salary: "+employee.salary);
            }
        }

        public IRestResponse GetEmployeeDetails()
        {
            //Creates GET request for accessing "/employees", for sending the request to the server
            RestRequest request = new RestRequest("/employees", Method.GET);

            //Sends the request to the server and in return gets the response which is collected by IRestResponse 
            IRestResponse response = restClient.Execute(request);

            return response;
        }

        /// <summary>
        /// TC2 Add a new Employee data into JSON server
        /// </summary>
        [TestMethod]
        public void AddaEmployeeIntoJSONServer()
        {
            RestRequest request = new RestRequest("/employees", Method.POST);

            //Creating new Emp Json data for adding into server
            JObject jObject = new JObject();
            jObject.Add("name","Kunal");
            jObject.Add("salary","20000");

            request.AddParameter("application/json", jObject, ParameterType.RequestBody);

            IRestResponse response = restClient.Execute(request);

            //StatusCode for Adding data is 201
            Assert.AreEqual(response.StatusCode, HttpStatusCode.Created);

            EmployeePayroll employees = JsonConvert.DeserializeObject<EmployeePayroll>(response.Content);

            Assert.AreEqual("Kunal", employees.name);

            Assert.AreEqual("20000", employees.salary);

            Console.WriteLine(response.Content);

        }


        /*public void AddMultipleEmployeesIntoJSONServer()
        {
            RestRequest request = new RestRequest("/employees", Method.POST, DataFormat.Json);

            List<object> input = new List<object>();

            var employees = new[] { new { name = "Rohit", salary = "100000" }, new { name = "Suresh", salary = "80000" } };

            

            //This method sets content type to application/json and serializes the object to a JSON string.
            //request.AddJsonBody(new { name = "Rohit", salary = "100000" });
            //request.AddJsonBody(new { name = "Suresh", salary = "80000" });

            //EmployeePayroll[] employees = new EmployeePayroll[]{ new EmployeePayroll{ name = "Rohit", salary = "100000" } , new EmployeePayroll { name = "Suresh", salary = "80000" } };

            input.Add(employees);

            request.AddJsonBody(input);

            /*JObject jObject1 = new JObject();
            jObject1.Add("name","Rohit");
            jObject1.Add("salary", "100000");

            request.AddParameter("application/json", jObject1, ParameterType.RequestBody);

            JObject jObject2 = new JObject();
            jObject2.Add("name","Suresh");
            jObject2.Add("salary","80000");*/

            //request.AddParameter("application/json", jObject2, ParameterType.RequestBody);


            //request.AddJsonBody(employee1);

            /*string json = JsonConvert.SerializeObject(employees);

            request.AddParameter(json, "application/json", ParameterType.RequestBody);*/

            //IRestResponse response = restClient.Execute(request);

            //var records = JsonConvert.DeserializeObject<object>(response.Content);

            //List<object> records = JsonConvert.DeserializeObject<List<object>>(response.Content);

            /*Assert.AreEqual("Rohit", records[0].name);

            Assert.AreEqual("100000", records[0].salary);

            Assert.AreEqual("Suresh", records[1].name);

            Assert.AreEqual("80000", records[1].salary);*/

            /*Assert.AreEqual("Rohit" ,records.Where(x => x.name.Equals("Rohit")).Select(x=>x.name));

            Assert.AreEqual("100000", records.Where(x => x.name.Equals("Rohit")).Select(x=>x.salary));

            Assert.AreEqual("Suresh", records.Where(x => x.name.Equals("Suresh")).Select(x => x.name));

            Assert.AreEqual("80000", records.Where(x => x.name.Equals("Suresh")).Select(x => x.salary));*/

            //Console.WriteLine(response.Content);

        //}

        /// <summary>
        /// TC4 Update Salary of Employee
        /// </summary>
        [TestMethod]
        public void UpdateSalaryIntoJSONServer()
        {
            RestRequest request = new RestRequest("/employees/11", Method.PUT);

            JObject jObject = new JObject();
            jObject.Add("name", "Tanmay");
            jObject.Add("salary", "30000");

            request.AddParameter("application/json", jObject, ParameterType.RequestBody);

            IRestResponse response = restClient.Execute(request);

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);

            EmployeePayroll employee = JsonConvert.DeserializeObject<EmployeePayroll>(response.Content);

            Assert.AreEqual("Tanmay", employee.name);

            Assert.AreEqual("30000", employee.salary);

            Console.WriteLine(response.Content);

        }

    }
}
