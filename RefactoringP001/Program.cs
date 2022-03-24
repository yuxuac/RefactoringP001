using System;

namespace RefactoringP001
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee employee = new Employee() 
            { 
                Id = 1, 
                Salary = new Salary() 
                { 
                    Value = 10000m 
                }, 
                EmployeeType = EmployeeType.FULL_TIME 
            };

            SalaryCalculator calc = new SalaryCalculator();
            var salary = calc.calculateSalary(employee);

            Console.WriteLine(salary.Value);
            Console.ReadLine();
        }
    }

    public class SalaryCalculator
    {
        public Salary calculateSalary(Employee employee)
        {
            decimal taxDecution = calculateTax(employee);
            employee.Salary.Value -= taxDecution;
            return employee.Salary;
        }

        private decimal calculateTax(Employee employee)
        {
            decimal result = 0;
            switch (employee.EmployeeType)
            {
                case EmployeeType.FULL_TIME:
                    result = employee.Salary.Value * 0.1m;
                    break;
                case EmployeeType.FREELANCE:
                    result = employee.Salary.Value * 0.05m;
                    break;
                default:
                    result = employee.Salary.Value * 0.1m;
                    break;
            }
            return result;
        }
    }

    public enum EmployeeType
    {
        FULL_TIME,
        FREELANCE
    }

    public class Employee
    {
        public long Id { get; set; }
        public EmployeeType EmployeeType { get; set; }
        public Salary Salary { get; set; }
    }

    public class Salary
    {
        public decimal Value { get; set; }
    }
}
