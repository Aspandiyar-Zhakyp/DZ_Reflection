using System;
using System.Collections.Generic;
using System.Reflection;

namespace Reflection
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the path to the dll/exe file: ");
            var path = Console.ReadLine();
            var styleDictionary = new Dictionary<string, string>(6);
            styleDictionary.Add("System.Char", "nvarchar");
            styleDictionary.Add("System.String", "nvarchar");
            styleDictionary.Add("System.Int32", "int"); 
            styleDictionary.Add("System.Double", "float");
            styleDictionary.Add("System.Single", "float");
            styleDictionary.Add("System.Boolean", "bool");

            Assembly myLibrary = Assembly.LoadFile(path);

            foreach (var style in myLibrary.GetTypes())
            {
                if (style.IsClass && !style.IsAbstract && style is Object)
                {
                    Console.WriteLine($"create table {style.Name}s (");
                    var styleLength = style.GetMembers().Length;
                    var Сounter = 0;
                    foreach (var participant in style.GetMembers())
                    {
                        if (participant is PropertyInfo)
                        {
                            Сounter++;
                            var propertyInfo = participant as PropertyInfo;
                            foreach (var cell in styleDictionary)
                            {
                                if (cell.Key == propertyInfo.PropertyType.ToString())
                                {
                                    Console.Write($"{propertyInfo.Name}, {cell.Value} ");
                                    if (propertyInfo.Name.ToString() == "Id")
                                    {
                                        Console.Write("null, primary key, identity");
                                    }
                                    else if (cell.Value == "nvarchar")
                                    {
                                        Console.Write("not null");
                                    }
                                    else if (cell.Value == "float" || cell.Value == "int" || cell.Value == "bool")
                                    {
                                        Console.Write("null");
                                    }
                                    styleLength -= 1;
                                    if (styleLength != 4)
                                    {
                                        Console.Write(";");
                                    }
                                    Console.WriteLine();
                                }
                            }
                        }
                    }
                    Console.WriteLine(")");
                    var methodInfo = style.GetMethod("GetClassName");
                    var instance = Activator.CreateInstance(style);
                    var result = methodInfo.Invoke(instance, new object[0]);
                }
            }
        }
    }
}
