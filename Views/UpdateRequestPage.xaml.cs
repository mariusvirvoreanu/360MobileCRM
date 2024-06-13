using CRM_App.Data;
using CRM_App.Models;
using Microsoft.EntityFrameworkCore;

namespace CRM_App.Views;


[QueryProperty(nameof(RequestId), "RequestId")]
public partial class UpdateRequestPage : ContentPage
{
    private int _requestId;
    public int RequestId
    {
        get => _requestId;
        set
        {
            _requestId = value;
        }
    }
    public UpdateRequestPage()
	{
		InitializeComponent();
        BindingContext = this;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        LoadRequestDetails();
    }
    private async void LoadRequestDetails()
    {
        var request = DatabaseHelper.GetRequestById(RequestId);
        if (request == null)
        {
            await DisplayAlert("Eroare", "Nu am putut incarca date solicitare client", "Ok");
        }
        else
        {
            CustomerName.Text = DatabaseHelper.GetCustomerByIdAsync(request.CustomerID).Result.Name;
            DescriptionEntry.Text = request.Description;
            StatusPicker.SelectedItem = request.Status;
            CommentsCollectionView.ItemsSource = await DatabaseHelper.GetCommentsReportForRequest(RequestId);
        }
        
    }
    private async void OnUpdateRequestClicked(object sender, EventArgs e)
    {
        bool isValid = ValidateFields();
        if (!isValid)
        {
            return;
        }
        var request = DatabaseHelper.GetRequestById(RequestId);
        if (request == null)
        {
            await DisplayAlert("Eroare", "Nu am gasit solicitarea clientului", "Ok");
        }
        else
        {
            request.Description = DescriptionEntry.Text;
            request.Status = StatusPicker.SelectedItem.ToString();

            int result = await DatabaseHelper.UpdateRequestAsync(request);
            if (result == 0)
            {
                await DisplayAlert("Eroare", "Solicitarea clientului nu a fost actualizata", "Ok");
            }
            else
            {
                await DisplayAlert("Info", "Solicitare actualizata cu succes", "Ok");
                await Navigation.PopAsync();
            }
        }
    }
    private async void OnDeleteRequestClicked(object sender, EventArgs e)
    {
        //1st delete comments
        try 
        { 
            await DatabaseHelper.DeleteCommentsAsync(RequestId); 
        }
        catch 
        { 
            await DisplayAlert("Eroare", "Nu am putut sterge comentariile din solicitare", "Ok");
            return;
        }
        //2nd delete request
        int result = await DatabaseHelper.DeleteRequestAsync(RequestId);
        if (result == 0)
        {
            await DisplayAlert("Eroare", "Nu am putut sterge solicitarea clientului", "Ok");
        }
        else
        {
            await DisplayAlert("Info", "Solicitarea clientului a fost stearsa", "Ok");
            await Shell.Current.Navigation.PopAsync();
        }
        
    }
    private async void OnAddCommentClicked(object sender, EventArgs e)
    {
        bool isValid = ValidatComment();
        if (!isValid)
        {
            return;
        }

        var comment = new Comment
        {
            RequestID = RequestId,
            UserID = AuthenticationManager.LoggedUser.UserID,
            CommentText = CommentEntry.Text,
            Timestamp = DateTime.Now
        };

        int result = await DatabaseHelper.AddCommentAsync(comment);
        if (result == 0)
        {
            await DisplayAlert("Eroare", "Nu am putut adauga comentariul", "Ok");
        }
        else
        {
            CommentsCollectionView.ItemsSource = null; // Refresh the collection view
            CommentsCollectionView.ItemsSource = await DatabaseHelper.GetCommentsReportForRequest(RequestId);
            CommentEntry.Text = string.Empty;
            await DisplayAlert("Info", "Comentariul a fost adaugat", "Ok");
        }
    }
    private bool ValidateFields()
    {
        bool isValid = true;
        // Validate Product
        if (string.IsNullOrWhiteSpace(StatusPicker.SelectedItem?.ToString()))
        {
            StatusPickerError.Text = "Selectati statusul!";
            StatusPickerError.IsVisible = true;
            isValid = false;
        }
        else
        {
            StatusPickerError.IsVisible = false;
        }
        // Validate Description
        if (string.IsNullOrWhiteSpace(DescriptionEntry.Text))
        {
            DescriptionError.Text = "Completati descrierea!";
            DescriptionError.IsVisible = true;
            isValid = false;
        }
        else
        {
            DescriptionError.IsVisible = false;
        }

        return isValid;
    }
    private bool ValidatComment()
    {
        bool isValid = true;
        // Validate Product
        if (string.IsNullOrWhiteSpace(CommentEntry.Text))
        {
            CommentError.Text = "Adaugati comentariul!";
            CommentError.IsVisible = true;
            isValid = false;
        }
        else
        {
            CommentError.IsVisible = false;
        }
     
        return isValid;
    }
}