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
    }
}
