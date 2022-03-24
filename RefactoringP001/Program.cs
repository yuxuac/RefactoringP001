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

            // ITaxCalculator can be replace with different implementations based on employee.EmployeeType
            //   which follows open close principle - open to extension, close to modification
            ITaxCalculator taxCalculator = employee.EmployeeType == EmployeeType.FREELANCE ? 
                                                    new FreelanceTaxCalculator() : 
                                                    new FullTimeTaxCalculator(); 

            SalaryCalculator calc = new SalaryCalculator(taxCalculator);
            var salary = calc.calculateSalary(employee);

            Console.WriteLine(salary.Value);
            Console.ReadLine();
        }
    }

    // Only responsible for calculate salary
    public class SalaryCalculator
    {
        private ITaxCalculator taxCalculator;
        public SalaryCalculator(ITaxCalculator taxCalculator)
        {
            this.taxCalculator = taxCalculator;
        }

        public Salary calculateSalary(Employee employee)
        {
            decimal taxDecution = taxCalculator.calculateTax(employee);
            employee.Salary.Value -= taxDecution;
            return employee.Salary;
        }
    }

    // Responsible for calculate tax deduction
    public interface ITaxCalculator
    {
        decimal calculateTax(Employee employee);
    }

    public class FullTimeTaxCalculator : ITaxCalculator
    {
        public decimal calculateTax(Employee employee)
        {
            return employee.Salary.Value * 0.1m;
        }
    }

    public class FreelanceTaxCalculator : ITaxCalculator
    {
        public decimal calculateTax(Employee employee)
        {
            return employee.Salary.Value * 0.05m;
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
