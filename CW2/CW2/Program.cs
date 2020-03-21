using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace CW2
{
 
    public class ActiveStudies
    {
        public string Name { get; set; }
        public int CountOfStudents { get; set; }
    }

    public class Studies
    {
        public string name { get; set; }
        public string mode { get; set; }
    }
     
    public class Uczelnia
    { 
        public string createdAt = DateTime.UtcNow.ToString("dd.MM.yyyy");
        public string author = "Taras Kostiuk";
        public HashSet<Student> studenci = new HashSet<Student>();
        public List<ActiveStudies> ActiveSt = new List<ActiveStudies>();

        public void AddActiveStudies(ActiveStudies studies)
        {
            ActiveSt.Add(studies);
        }

        public void AddNewStudents(Student student)
        {
            studenci.Add(student);
        }
    }

    public class Student
    {
        public string fname { get; set; }
        public string lname { get; set; }
        public string name { get; set; }
        public string mode { get; set; }
        [XmlAttribute]
        public string indexNumber { get; set; }
        public string birthdate { get; set; }
        public string email { get; set; }
        public string mothersName { get; set; }
        public string fathersName { get; set; }
        public Studies studies { get; set; }
    }

    public class StudentComparer : IEqualityComparer<Student>
    {
        public bool Equals(Student x, Student y)
        {
            return StringComparer
                .InvariantCultureIgnoreCase
                .Equals(
                    $"{x.fname} {x.lname} {x.indexNumber}",
                    $"{y.fname} {y.lname} {y.indexNumber}");
        }

        public int GetHashCode(Student o)
        {
            return StringComparer
                .CurrentCultureIgnoreCase
                .GetHashCode($"{o.fname} {o.lname} {o.indexNumber}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            string add1 = args.Length > 0 ? args[0] : @"dane.csv";
            string add2 = args.Length > 1 ? args[1] : @"Data/test.xml";
            string type = args.Length > 2 ? args[2] : "xml";



            StreamWriter sw = new StreamWriter(@"Data/Errors.txt");

            var students = new HashSet<Student>(new StudentComparer());
            var studies = new HashSet<ActiveStudies>();

            if (!File.Exists(add1) || !File.Exists(add2))
            {
                try
                {
                    throw new FileNotFoundException("File does not exist");
                }
                catch (FileNotFoundException ex)
                {
                    sw.WriteLine($"{ex.Message}");
                    sw.Close();
                    return;
                }
            }

            Uczelnia ucz = new Uczelnia();
            int CSStud = 0;
            int MAstud = 0;

            var plik = new FileInfo(add1);
            using (var stream = new StreamReader(File.OpenRead(Path.GetFullPath(add1))))
            {
                string line = null;
                while ((line = stream.ReadLine()) != null)
                {
                    string[] data = line.Split(',');

                    if (data.Length != 9)
                    {
                        try
                        {
                            throw new FormatException("Wrong data format");
                        }
                        catch (FormatException ex)
                        {
                            sw.WriteLine($"{line} - {ex.Message}");
                        }
                    }
                    else
                    {
                        bool check = true;
                        for (int i = 0; i < data.Length; i++)
                        {
                            if (data[i].Equals(""))
                            {
                                check = false;
                                try
                                {
                                    throw new FormatException("Wrong data format");
                                }
                                catch (FormatException ex)
                                {
                                    sw.WriteLine($"{line} - {ex.Message}");
                                }
                                break;
                            }
                        }

                        if (check)
                        {

                            if (data[2].Contains("Informatyka"))
                            {
                                CSStud++;
                            }
                            else
                            {
                                MAstud++;
                            }

                            var student = new Student
                            {
                                fname = data[0],
                                lname = data[1],
                                //  name = data[2],
                                //  mode = data[3],
                                indexNumber = $"s{data[4]}",
                                birthdate = data[5],
                                email = data[6],
                                mothersName = data[7],
                                fathersName = data[8],
                                studies = new Studies()
                                {
                                    name = data[2],
                                    mode = data[3],
                                }
                            };
                            ucz.AddNewStudents(student);

                        }
                    }
                }

            }
            sw.Close();

            ActiveStudies acts = new ActiveStudies { Name = "Computer Science", CountOfStudents = CSStud };
            ActiveStudies acts2 = new ActiveStudies { Name = "New Media Art", CountOfStudents = MAstud };
            ucz.AddActiveStudies(acts);
            ucz.AddActiveStudies(acts2);
          
            FileStream writer = new FileStream(add2, FileMode.Create);
            XmlSerializer serializer = new XmlSerializer(typeof(Uczelnia));
            serializer.Serialize(writer, ucz);
        }
    }
}