using System.Collections;
using ExpenseTracker.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ExpenseTracker.Repository.Helpers;

namespace ExpenseTracker.Repository.Factories
{
    public class ExpenseGroupFactory
    {
        ExpenseFactory expenseFactory = new ExpenseFactory();

        public ExpenseGroupFactory()
        {

        }

        public ExpenseGroup CreateExpenseGroup(DTO.ExpenseGroup expenseGroup)
        {
            return new ExpenseGroup()
            {
                Description = expenseGroup.Description,
                ExpenseGroupStatusId = expenseGroup.ExpenseGroupStatusId,
                Id = expenseGroup.Id,
                Title = expenseGroup.Title,
                UserId = expenseGroup.UserId,
                Expenses = expenseGroup.Expenses == null ? new List<Expense>() : expenseGroup.Expenses.Select(e => expenseFactory.CreateExpense(e)).ToList()
            };
        }


        public DTO.ExpenseGroup CreateExpenseGroup(ExpenseGroup expenseGroup)
        {
            return new DTO.ExpenseGroup()
            {
                Description = expenseGroup.Description,
                ExpenseGroupStatusId = expenseGroup.ExpenseGroupStatusId,
                Id = expenseGroup.Id,
                Title = expenseGroup.Title,
                UserId = expenseGroup.UserId,
                Expenses = expenseGroup.Expenses.Select(e => expenseFactory.CreateExpense(e)).ToList()
            };
        }

        public object CreateDataShapedObject(ExpenseGroup expenseGroup, List<string> fields)
        {
            return CreateDataShapedObject(CreateExpenseGroup(expenseGroup), fields);
        }

        public object CreateDataShapedObject(DTO.ExpenseGroup expenseGroup, List<string> listOfFields)
        {
            //works with a new instance, as we'll manipulate this list in this method
            List<string> fieldsToWorkWith = new List<string>(listOfFields);

            if (!fieldsToWorkWith.Any())
                return expenseGroup;
            else
            {
                //does it include any expense-related field?
                var expenseFields = fieldsToWorkWith.Where(f => f.Contains("expenses")).ToList();

                //if one of those fields is "expenses", we need to ensure the FULL expense is returned. If
                //it's only subfields, only those subfields have to be returned.

                bool returnPartialExpense = expenseFields.Any() && !expenseFields.Contains("expenses");

                //if we don't want to return the full expense, we need to know which fields
                if (returnPartialExpense)
                {
                    //remove all expense-related fields from list of fields,
                    // as we will use the CreateDataShapedObject function in ExpenseFactory for that.

                    fieldsToWorkWith.RemoveRange(expenseFields);
                    expenseFields = expenseFields.Select(f => f.Substring(f.IndexOf(".") + 1)).ToList();
                }

                else
                {
                    //we shouldn't return a partial expense, but the consumer might still have 
                    // asked for a subfield together with the main field, ie: expense, expense.id. We
                    //need to remove those subfields in that case.

                    expenseFields.Remove("expenses");
                    fieldsToWorkWith.RemoveRange(expenseFields);
                }

                //create a new ExpandoObject & dynamically create the properties for this object

                //if we have an expense

                ExpandoObject objectToReturn = new ExpandoObject();
                foreach (var field in fieldsToWorkWith)
                {
                    //need to include public and instance, b/c specifying a binding flag overwrites the 
                    // already-existing binding flags.

                    var fieldValue = expenseGroup.GetType()
                                                 .GetProperty(field,
                                                              BindingFlags.IgnoreCase | BindingFlags.Public |
                                                              BindingFlags.Instance)
                                                 .GetValue(expenseGroup, null);
                    //add the field to the ExpandoObject
                    ((IDictionary<String, Object>)objectToReturn).Add(field,fieldValue);
                }


                if (returnPartialExpense)
                {
                    // add a list of expenses, and in that , add all those expenses
                    List<object> expenses = new List<object>();
                    foreach (var expense in expenseGroup.Expenses)
                    {
                        expenses.Add(expenseFactory.CreateDataShapedObject(expense, expenseFields));
                    }

                    ((IDictionary<String, Object>) objectToReturn).Add("expenses", expenses);
                }

                return objectToReturn;

            }
        }

         
         
    }
}
