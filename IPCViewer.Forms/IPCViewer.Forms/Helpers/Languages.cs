namespace IPCViewer.Forms.Helpers
{
    using Interfaces;
    using Resources;
    using Xamarin.Forms;

    /***
     * Clase donde almacenará todas las cadenas de texto para traducirlas
     */

    public static class Languages
    {
        static Languages ()
        {
            var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
            Resource.Culture = ci;
            DependencyService.Get<ILocalize>().SetLocale(ci);
        }

        // LOGIN Y REGISTER
        public static string Accept => Resource.Accept;

        public static string Error => Resource.Error;

        public static string EmailError => Resource.EmailError;

        public static string PasswordError => Resource.PasswordError;

        public static string LoginError => Resource.LoginError;

        public static string Welcome => Resource.Welcome;

        public static string PasswordPlaceHolder => Resource.PasswordPlaceHolder;

        public static string Login => Resource.Login;

        public static string RememberMe => Resource.RememberMe;

        public static string RegisterTitle => Resource.RegisterTitle;

        public static string FirstnamePlaceHolder => Resource.FirstnamePlaceHolder;

        public static string CitySelect => Resource.CitySelect;

        public static string Register => Resource.Register;

        public static string Firstname => Resource.Firstname;

        public static string Username => Resource.Username;

        public static string ErrorValidEmail => Resource.ErrorValidEmail;

        public static string ErrorRegister => Resource.ErrorRegister;

        public static string RegisterSuccess => Resource.RegisterSuccess;


        // ------------------ ADD CAMERA

        public static string NameCameraPlaceholder => Resource.NameCameraPlaceholder;

        public static string CommentsCameraPlaceholder => Resource.CommentsCameraPlaceholder;

        public static string SelectCityCameraPlaceholder => Resource.SelectCityCameraPlaceholder;

        public static string LatitudeCameraPlaceholder => Resource.LatitudeCameraPlaceholder;

        public static string LongitudeCameraPlaceholder => Resource.LongitudeCameraPlaceholder;

        public static string SaveCamera => Resource.SaveCamera;

        public static string ErrorLoadCities => Resource.ErrorLoadCities;
        public static string ErrorCameraName => Resource.ErrorCameraName;
        public static string ErrorCameraLatitude => Resource.ErrorCameraLatitude;
        public static string ErrorCameraLongitude => Resource.ErrorCameraLongitude;
        public static string ErrorCameraCity => Resource.ErrorCameraCity;
        public static string Cancel => Resource.Cancel;
        public static string Alert => Resource.Alert;
        public static string AlertImageCamera => Resource.AlertImageCamera;
        public static string ErrorCreateCamera => Resource.ErrorCreateCamera;
        public static string DisplayActionImage => Resource.DisplayActionImage;
        public static string FromGallery => Resource.FromGallery;
        public static string FromCamera => Resource.FromCamera;
        public static string FromUrl => Resource.FromUrl;

        // ------------------ ADD LOCATION

        public static string HybridView => Resource.HybridView;

        public static string SatelliteView => Resource.SatelliteView;
        
        public static string ErrorCameraLocation => Resource.ErrorCameraLocation;

        public static string AlertEmptyImage => Resource.AlertEmptyImage;

        public static string OwnLocation => Resource.OwnLocation;

        public static string ErrorSelectPin => Resource.ErrorSelectPin;

        // ------------------ ADD URL

        public static string CameraUrlPlaceholder => Resource.CameraUrlPlaceholder;

        // ------------------ DISPLAY CAMERA
        public static string LatitudeLabel => Resource.LatitudeLabel;

        public static string LongitudeLabel => Resource.LongitudeLabel;

        public static string CityLabel => Resource.CityLabel;

        public static string CreatedByLabel => Resource.CreatedByLabel;

        public static string DateCreationLabel => Resource.DateCreationLabel;

        // ------------------ EDIT CAMERA
        public static string DeleteCamera => Resource.DeleteCamera;
        public static string ErrorEditCamera => Resource.ErrorEditCamera;

        // ------------------ MAPS PAGE

        public static string NameLabel => Resource.NameLabel;
        public static string ErrorUserLocation => Resource.ErrorUserLocation;

        // ------------------ PROFILE PAGE

        public static string DarkMode => Resource.DarkMode;

        public static string ConfirmPasswordPlaceholder => Resource.ConfirmPasswordPlaceholder;

        public static string NewPasswordPlaceholder => Resource.NewPasswordPlaceholder;

        public static string CurrentPasswordPlaceholder => Resource.CurrentPasswordPlaceholder;

        public static string ErrorEmptyFirstname => Resource.ErrorEmptyFirstname;

        public static string ErrorEmptyUsername => Resource.ErrorEmptyUsername;

        public static string ErrorModifyUser => Resource.ErrorModifyUser;

        public static string ModifySuccess => Resource.ModifySuccess;
        
        public static string ErrorCurrentPassword => Resource.ErrorCurrentPassword;

        public static string ErrorCurrentPasswordIncorrect => Resource.ErrorCurrentPasswordIncorrect;

        public static string ErrorNewPassword => Resource.ErrorNewPassword;

        public static string ErrorPasswordLength => Resource.ErrorPasswordLength;

        public static string ErrorEmptyPasswordConfirm => Resource.ErrorEmptyPasswordConfirm;

        public static string ErrorPasswordsDoesntMatch => Resource.ErrorPasswordsDoesntMatch;

        public static string ErrorModifyPassword => Resource.ErrorModifyPassword;

        public static string ModifyPasswordSuccess => Resource.ModifyPasswordSuccess;

        // ------------------ CAMERAS PAGE

        public static string NumCameras => Resource.NumCameras;

        public static string SearchCamera => Resource.SearchCamera;

        public static string ErrorLoadCameras => Resource.ErrorLoadCameras;


        // ------------------ MENU PAGE

        public static string Maps=> Resource.Maps;
        public static string ModifyUser => Resource.ModifyUser;
        public static string About => Resource.About;
        public static string CloseSession => Resource.CloseSession;

        // ------------------ TITLES
        public static string ViewCameraTitle => Resource.ViewCameraTitle;
        public static string ProfileTitle => Resource.ProfileTitle;
        public static string AddCameraTitle => Resource.AddCameraTitle;
        public static string EditCameraTitle => Resource.EditCameraTitle;
        public static string CamerasTitle => Resource.CamerasTitle;
        public static string MapsTitle => Resource.MapsTitle;
        public static string AddLocationTitle => Resource.AddLocationTitle;

        // ------------------ POP UP CAMERA
        public static string ViewCamera => Resource.ViewCamera;
        public static string ViewMaps => Resource.ViewMaps;
        public static string EditCamera => Resource.EditCamera;

    }
}