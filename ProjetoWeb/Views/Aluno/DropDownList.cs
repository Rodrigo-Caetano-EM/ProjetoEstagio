using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjetoWeb.Views.Aluno
{
    public class DropDownListViewModel
    {
        public string? SelectedValue { get; set; }
        public IEnumerable<SelectListItem>? Items { get; set; }
    }
}
