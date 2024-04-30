
namespace COP4870_Canvas_Clone;
using COP4870_Canvas_Clone.Models;


public partial class TeacherDashboardPage : ContentPage
{
    public TeacherDashboardPage()
    {
        InitializeComponent();
    }

    private void OnAddStudentClicked(object sender, EventArgs e)
    {
        // Navigate to Add Student Page or show a form
    }

    private void OnAddCourseClicked(object sender, EventArgs e)
    {
        // Navigate to Add Course Page or show a form
    }

    private void OnManageRostersClicked(object sender, EventArgs e)
    {
        // Navigate to a Page to select a course and then manage its roster
    }

    private void OnAddModulesClicked(object sender, EventArgs e)
    {
        // Navigate to a Page to select a course and then add modules to it
    }

    private async void OnCourseSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is Course selectedCourse)
        {
            // Assuming CourseDetailPage takes a Course object as its context
            var detailPage = new CourseDetailPage(selectedCourse);
            await Navigation.PushAsync(detailPage);
            // Deselect item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
