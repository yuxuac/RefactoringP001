using System;

namespace RefactoringP001
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee[] employees = new Employee[]
            {
                new Employee()
                {
                    Id = 1,
                    Salary = new Salary() { Value = 10000m },
                    EmployeeType = EmployeeType.FULL_TIME
                },
                new Employee()
                {
                    Id = 2,
                    Salary = new Salary() { Value = 11000m },
                    EmployeeType = EmployeeType.FREELANCE
                },
            };

            SalaryCalculator calc = new SalaryCalculator(new TaxCalculatorFactory());
            foreach (var employee in employees)
            {
                var salary = calc.calculateSalary(employee);
                Console.WriteLine(salary.Value);
            }
            
            Console.ReadLine();
        }
    }

    // Only responsible for calculate salary
    public class SalaryCalculator
    {
        private readonly TaxCalculatorFactory taxCalculatorFactory;
        private ITaxCalculator taxCalculator;
        public SalaryCalculator(TaxCalculatorFactory taxCalculatorFactory)
        {
            this.taxCalculatorFactory = taxCalculatorFactory;
        }

        public Salary calculateSalary(Employee employee)
        {
            if (taxCalculator == null)
                taxCalculator = taxCalculatorFactory.newTaxCalculator(employee);

            decimal taxDecution = taxCalculator.calculateTax(employee);
            employee.Salary.Value -= taxDecution;
            return employee.Salary;
        }
    }

    // Only the factory will be changed when new EmployeeType introduced which minimize the code change
    public class TaxCalculatorFactory
    {
        // ITaxCalculator can be replace with different implementations based on employee.EmployeeType
        //   which follows open close principle - open to extension, close to modification
        public ITaxCalculator newTaxCalculator(Employee employee)
        {
            ITaxCalculator taxCalculator;
            switch (employee.EmployeeType)
            {
                case EmployeeType.FREELANCE:
                    taxCalculator = new FreelanceTaxCalculator();
                    break;
                case EmployeeType.FULL_TIME:
                    taxCalculator = new FullTimeTaxCalculator();
                    break;
                default:
                    taxCalculator = new FullTimeTaxCalculator();
                    break;
            }
            return taxCalculator;
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
