namespace avanphamaceuticalsmanagement.Client.Pages.Administration
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
        public string RoleName { get;set; }
    }
}
