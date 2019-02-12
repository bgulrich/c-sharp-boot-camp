using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Generics
{
    public class Person 
    {
        public string Name { get; set; }
    }

    public class Employee : Person
    {
        public virtual string DoWork() { return $"{Name} is doing some work."; }
    }

    public class Developer : Employee
    {
        public override string DoWork() { return $"{Name} is writing some code."; }
    }

    public class CSharpDeveloper : Developer
    {
        public override string DoWork() { return $"{Name} is using yield returns."; }
    }

    public class Manager : Employee
    {
        public override string DoWork() { return $"{Name} is creating a meeting."; }
    }


    /// <summary>
    /// Covariance (out)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IReadOnlyEmployeeCollection<out T> where T : Employee
    {
        IEnumerable<T> GetAll();
    }

    /// <summary>
    /// Contravariance (in)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IWriteOnlyEmployeeCollection<in T> where T : Employee
    {
        void Add(T item);
    }

    public class Workers<T> : IReadOnlyEmployeeCollection<T>, IWriteOnlyEmployeeCollection<T> where T : Employee
    {
        List<T> _workers = new List<T>();

        public virtual void Add(T item)
        {
            _workers.Add(item);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _workers;
        }
    }
}
