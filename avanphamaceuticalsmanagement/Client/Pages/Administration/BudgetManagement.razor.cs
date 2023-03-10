using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Reflection;
using avanphamaceuticalsmanagement.Client.Services.Interfaces;
using avanphamaceuticalsmanagement.Shared.Models;

namespace avanphamaceuticalsmanagement.Client.Pages.Administration
{
    public partial class BudgetManagement
    {
        protected string AlertText = string.Empty;
        protected bool buttonClicked = false;
        protected bool isAddBudget = false;
        static DateTime BudgetBeginDate = DateTime.Now;
        static DateTime BudgetEndDate = DateTime.Now.AddDays(4);
        protected BudgetsTable Budget = new();
        protected BudgetTypeTable budgetType = new();
        protected IList<BudgetTypeTable> _budgetTypes = new List<BudgetTypeTable>();
        protected IList<BudgetsTable> _budgets = new List<BudgetsTable>();
        [Inject] IGenericService _genericService { get; set; }
        [Inject] ISnackbar Snackbar { get; set; }
        [Inject] NavigationManager Navigation { get; set; }
        protected DateRange _dateRange = new DateRange(BudgetBeginDate, BudgetEndDate);
        protected MudAlert mudAlert = new();
        protected BudgetsTable selectedItem = new();
        protected BudgetsTable backupItem = new();
        protected BudgetsTable elementBeforeEdit = new();
        protected List<string> editEvents = new();
        protected string searchString = string.Empty;
        protected TableApplyButtonPosition applyButtonPosition = TableApplyButtonPosition.End;
        protected TableEditButtonPosition editButtonPosition = TableEditButtonPosition.End;

        protected async Task GetAddBudgetForm()
        {
            mudAlert.Severity = Severity.Info;
            AlertText = "Fill in Required Information to add Budget";
            var result = await _genericService.GetAllAsync<BudgetTypeTable>("api/AvanPharmacy/GetBudgetTypes");
            _budgetTypes = result.ToList();
            buttonClicked = true;
            isAddBudget = true;



        }

        protected async Task EditExistingBudget()
        {
            mudAlert.Severity = Severity.Warning;
            AlertText = "Edit Budget Date. Remember to counter Check and maker Sure all information is correct";
            if(_budgetTypes.Count == 0)
            {
                var result = await _genericService.GetAllAsync<BudgetTypeTable>("api/AvanPharmacy/GetBudgetTypes");
                _budgetTypes = result.ToList();

            }
            var data = await _genericService.GetAllAsync<BudgetsTable>("api/AvanPharmacy/GetBudgets");
            _budgets = data.ToList();
            foreach (var item in _budgets)
            {
                item.BudgetType = _budgetTypes.Where(x => x.Id == item.BudgetTypeId).FirstOrDefault();
            }
            if (isAddBudget)
            {
                isAddBudget = false;
            }
            buttonClicked = true;
        }

        protected async Task SaveBudget()
        {
            Budget.StartDate = _dateRange.Start.Value;
            Budget.EndDate = _dateRange.End.Value;
            Budget.BudgetTypeId = budgetType.Id;
            var virtualProperties = Budget.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.GetGetMethod()?.IsVirtual == true);
            foreach (var virtualProperty in virtualProperties)
            {
                var value = virtualProperty.GetValue(Budget);
                if (value != null)
                {
                    virtualProperty.SetValue(Budget, null);
                }
            }
            try
            {
                await _genericService.SaveAllAsync("api/AvanPharmacy/SaveBudget", Budget);
                Snackbar.Add("Budget Saved", Severity.Success);
                Navigation.NavigateTo("/");
            }
            catch (Exception ex)
            {
                var _ = ex.Message;
                Snackbar.Add(_, Severity.Error);
                throw;
            }

        }

        #region InLine Table
        private void ClearEventLog()
        {
            editEvents.Clear();
        }

        private void AddEditionEvent(string message)
        {
            editEvents.Add(message);
            StateHasChanged();
        }

        private void BackupItem(object element)
        {
            elementBeforeEdit = new()
            {
                id = ((BudgetsTable)element).id,
                BudgetType = ((BudgetsTable)element).BudgetType,
                Amount = ((BudgetsTable)element).Amount,
                StartDate = ((BudgetsTable)element).StartDate,
                EndDate = ((BudgetsTable)element).EndDate,
                Comment = ((BudgetsTable)element).Comment,
            };
            AddEditionEvent($"RowEditPreview event: made a backup of Budget {((BudgetsTable)element).Amount}");
            Snackbar.Add($"made a backup of Budget {((BudgetsTable)element).Amount}", Severity.Info);
        }

        private void ItemHasBeenCommitted(object element)
        {
            AddEditionEvent($"RowEditCommit event: Changes to Budget {((BudgetsTable)element).Amount} committed");
            Snackbar.Add($"Changes to Budget {((BudgetsTable)element).Amount} committed", Severity.Success);
        }

        private void ResetItemToOriginalValues(object element)
        {
            ((BudgetsTable)element).id = elementBeforeEdit.id;
            ((BudgetsTable)element).BudgetType = elementBeforeEdit.BudgetType;
            ((BudgetsTable)element).Amount = elementBeforeEdit.Amount;
            ((BudgetsTable)element).StartDate = elementBeforeEdit.StartDate;
            ((BudgetsTable)element).EndDate = elementBeforeEdit.EndDate;
            ((BudgetsTable)element).Comment = elementBeforeEdit.Comment;
            AddEditionEvent($"RowEditCancel event: Editing Budget {((BudgetsTable)element).Amount} canceled");
            Snackbar.Add($"Editing Budget {((BudgetsTable)element).Amount} canceled", Severity.Error);
        }

        private bool FilterFunc(BudgetsTable element)
        {
            if (string.IsNullOrWhiteSpace(searchString))
                return true;
            if (element.Amount.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if (element.BudgetType.BudgetType.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                return true;
            if ($"{element.StartDate} {element.EndDate}".Contains(searchString))
                return true;
            return false;
        }
        #endregion

        #region String Functions
        protected Func<BudgetTypeTable, string> Budgetconverter = p => p.BudgetType;
        #endregion
    }
}