using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class mycode
{
    public string execute(HttpRequest request)
    {
        var input = new StreamReader(request.Body).ReadToEnd();
        var person = JsonConvert.DeserializeObject<Person>(input);

        return $"Hello {person.Name}!";
        // return DoSomeMath(4,5);
    }

    public int DoSomeMath(int x, int y) => x+y;

    public class Person{
        public string Name { get; set; }

        public Person(string name)
        {
            Name = name;
        }
    }
}