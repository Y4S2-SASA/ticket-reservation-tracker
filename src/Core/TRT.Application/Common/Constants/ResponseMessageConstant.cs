namespace TRT.Application.Common.Constants
{
    public static class ResponseMessageConstant
    {
        #region Common
        public const string COMMON_EXCEPTION_RESPONSE_MESSAGE =
                                "Error has been occured, please try again";

        #endregion


        #region Users

        public const string USER_DOES_NOT_EXIST_RESPONSE = "User does not exist in the system";

        public const string PASSWORD_INCORRECT_RESPONSE = "Password is incorrect";

        public const string USERNAME_NOT_VALID_EXCEPTION_RESPONSE_MESSAGE =
                                "User name not exising system, please try again";

        public const string PASSWORD_NOT_VALID_EXCEPTION_RESPONSE_MESSAGE =
                               "Password incorrect, please try again";

        public const string USER_DETAILS_SAVE_SUCCESS_RESPONSE_MESSAGE =
                               "User details saved has been successfull";

        public const string USER_DETAILS_UPDATE_SUCCESS_RESPONSE_MESSAGE =
                               "User details update has been successfull";

        public const string USER_DETAILS_DEACTIVE_SUCCESS_RESPONSE_MESSAGE =
                             "{0} deactive has been successfull";

        public const string USER_STATUS_CHANGE_SUCCESS_RESPONSE_MESSAGE =
                             "{0} {1} has been successfull";

        public const string USER_DETAILS_REACTIVE_SUCCESS_RESPONSE_MESSAGE =
                            "{0} reactive has been successfull";

        public const string USER_NOT_EXSISTING_THE_SYSTEM_RESPONSE_MESSAGE =
                               "User details Not found please try again";

        public const string USER_DELETE_SUCCESS_RESPONSE_MEESSAGE =
                              "User details delete has been successfull";


        #endregion


        #region Train
        public const string TRAIN_SAVE_SUCCESS_RESPONSE_MESSAGE =
                             "Train details saved has been successfully";

        public const string TRAIN_UPDATE_SUCCESS_RESPONSE_MESSAGE =
                            "Train details Updated has been successfully";

        public const string TRAIN_STATUS_CHANGE_SUCCESS_RESPONSE_MESSAGE =
                             "{0} {1} has been successfully";

        public const string TRAIN_NOT_EXSISTING_THE_SYSTEM_RESPONSE_MESSAGE =
                              "Train details Not found please try again";

        public const string CANNOT_TRAIN_STATUS_CHANGE =
                              "Cannot change train status";
        #endregion

        #region Schedule
        public const string SCHEDULE_SAVE_SUCCESS_RESPONSE_MESSAGE =
                             "Schedule details saved has been successfully";

        public const string SCHEDULE_UPDATE_SUCCESS_RESPONSE_MESSAGE =
                            "Schedule details Updated has been successfully";

        public const string SCHEDULE_STATUS_CHANGE_SUCCESS_RESPONSE_MESSAGE =
                             "Schedule status has been changed successfully";

        public const string SCHEDULE_NOT_EXSISTING_THE_SYSTEM_RESPONSE_MESSAGE =
                              "Schedule details Not found please try again";
        #endregion

        #region Reservation
        public const string RESERVATION_SAVE_SUCCESS_RESPONSE_MESSAGE =
                            "Reservation details saved has been successfully.";

        public const string UNABLE_TO_UPDATE_RESERVATION = 
            "Unable to Update Reservation\", \"We regret to inform you that we are unable to update your reservation as it is less than 5 days before the reservation date.";

        public const string RESERVATION_UPDATE_SUCCESS_RESPONSE_MESSAGE =
                           "Reservation details updated has been successfully.";

        public const string RESERVATION_STATUS_CHANGE_SUCCESS_RESPONSE_MESSAGE =
                           "Reservation status has been changed successfully.";

        public const string RESERVATION_NOT_EXSISTING_THE_SYSTEM_RESPONSE_MESSAGE =
                          "Reservation details Not found please try again.";

        public const string RESERVATION_DELETE_SUCCESS_RESPONSE_MESSAGE =
            "Reservation details deleted has been successfully.";
        #endregion
    }
}
