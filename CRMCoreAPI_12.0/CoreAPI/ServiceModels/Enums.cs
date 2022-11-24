using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMCoreAPI.ServiceModels
{
    public enum ECompanyStatus
    {
        None = 0,
        Active = 1,
        Inactive = 2,
        Pending = 3,
        InDunning = 4,
        InGrace = 5,
        Suspended = 6,
        Deactivated = 7,
        DeactivationPending = 8


    }

    public enum EUserStatus
    {
        Active = 1,
        Inactive = 2,
        Blocked = 3,
        Deleted = 4

    }

    /// <summary>
    ///  STRFRT,// If User Register from store Front
    ///    SPRADMN,// If Superadmin creates a new company
    ///    CLNTASCA,// If Client Want to be a CA
    ///   TRLTOPAID, // If User moves from free trial to paid
    ///    REACTVT // If user reactivate his company
    /// </summary>
    public enum RequestFrom
    {
        STRFRT, // If User Register from store Front
        SPRADMN, // If Superadmin creates a new company
        CLNTASCA, // If Client Want to be a CA
        TRLTOPAID, // If User moves from free trial to paid
        REACTVT // If user reactivate his company

    }

    public enum RegistrationType
    {
        Free = 1,
        Paid = 2,
        Enterprise = 3
    }

    public enum SubscriptionCreateType
    {
        SubscriptionOnly = 1,
        SubscriptionPayment = 2,
        SubscriptionPaymentActivation = 3,
        SubscriptionPaymentOfflineActivation = 4
    }

    public enum AddOnTypes
    {
        Linked = 1,
        Independent = 2,
        Customize = 3,
        Default = 4
    }
    public enum SubscriptionDiscountType
    {
        PercentDiscount = 1,
        FlatDiscount = 2
    }
    public enum SubscriptionType
    {
        Standard = 1,
        Enterprise = 2
    }
    public enum DiscountRuleType
    {
        LicenseBase = 1,
        AmountBase = 2
    }
    public enum SubscriptionPaymentOption
    {
        Online = 1,
        Offline = 2,
        SendaLink = 3,
        None = 4
    }

    public enum SubscriptionAccountStatus
    {
        Active = 1,
        Inactive = 2
    }
    public enum GenerateInvoice
    {
        No = 0,
        Yes = 1,
    }

    public enum CreditCardChargeType
    {
        PaymentOnly = 0,
        PaymentAndShceduling = 1

    }
    public enum ManageSubscriptionType
    {
        NEW = 1,
        PACKAGE = 2,
        AddON = 3

    }
    public enum InvoiceType
    {
        Initial = 1,
        Scheduled = 2,
        ProPackage = 3,
        ProAddon = 4,
        AfterCancel = 5

    }

    public enum PaymentGateway
    {
        Authorize,
        Intuit,
        Paypal,
        Stripe
    }
    public enum PaymentType
    {
        Manual,
        Transaction
    }

    public enum SubscriptionUpgradeType
    {
        License = 1,
        UpgradePackage = 2,
        DowngradePackage = 3
    }

    public class RegexExp
    {
        // Modified By: Surinderjit Singh
        // Modified At: 2015-08-05
        // Comment:     "=" is enabled as per the discussion with Hardeep Singh.        
        public const string Normal = @"^([^<>&]|&+[^#<>=])*&*$"; // Old Normal=@"^([^<>=&]|&+[^#<>=])*&*$";

        public const string Password = @"^([^<>=&]|&+[^#<>=])*&*$";// Old Regex @"^[^<>~!$%^&*+';=]*$";

        public const string Email = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";

        public const string MultipleEmail = @"/(([a-zA-Z0-9\-?\.?]+)@(([a-zA-Z0-9\-_]+\.)+)([a-z]{2,3})(\W?[,;]\W?(?!$))?)+$/i";

        public const string Rescricted = @"^([^<>';=&]|&+[^#<>';=])*&*$"; //Old Regex @"^([^<>';=&]|&+[^#<>';=])*&*$";

        public const string Captcha = @"^[^<>~!$&*+;=#/\%^]*$";

        public const string Phone = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

        public const string USAPhone = @"^[(+),(.),0-9 (,),-]+$";

        public const string USAZipcode = @"([a-zA-Z0-9@ ]+)";

        public const string FileName = @"^([^<>';=&.:/\\|?*]|&+[^#<>';=])*&*$";

        public const string Url = @"^(http|https|ftp)\://[a-zA-Z0-9\-\.]+\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";

        public const string IPAddress = @"^(([01]?\d\d?|2[0-4]\d|25[0-5])\.){3}([01]?\d\d?|25[0-5]|2[0-4]\d)$";
        // public const string IPAddress = @"^(?>(?>([a-f0-9]{1,4})(?>:(?1)){7}|(?!(?:.*[a-f0-9](?>:|$)){8,})((?1)(?>:(?1)){0,6})?::(?2)?)|(?>(?>(?1)(?>:(?1)){5}:|(?!(?:.*[a-f0-9]:){6,})(?3)?::(?>((?1)(?>:(?1)){0,4}):)?)?(25[0-5]|2[0-4][0-9]|1[0-9]{2}|[1-9]?[0-9])(?>\.(?4)){3}))$/i";

        // Created by: Surinderjit Singh
        // Created At: 2014-11-04
        // Used in AD Login
        // Accept any domain name e.g. google.com, yahoo.co.in etc.
        public const string Domain = @"^[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,6}$";

        // Created by: Surinderjit Singh
        // Created At: 2014-11-04
        // Used in AD Login
        // Accept any word character (letter, number, underscore)
        public const string EmailFirstPart = @"^([\w-\.]+)$";

        // Created by: Surinderjit Singh
        // Created At: 2014-12-29
        public const string Decimal = @"[-]?\d{1,18}(?:[,.]\d{1,2})?$";

    }


    public enum CFControlType
    {
        TextBox,
        SelectList,
        CheckBox,
        RadioButtonList,
        MultiSelect,
        CheckBoxList

    }

    public class Enumutil
    {
        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }

    /// <summary>
    /// Sukhdeep Kambo 
    /// Created on: May 02, 2016 
    /// Used to define the type of the integration
    /// </summary>
    public enum IntegrationType
    {
        Workday = 1,
        MSDynamicsCRM = 2
    }


    public enum AssetReplacementType
    {
        Lost = 1,
        Damage = 2,
        Renew = 3,
        Stolen = 4
    }

    public enum AssetReturnRequestByUserType
    {
        Manager = 1,
        User = 2
    }

    public enum MaintenanceType
    {
        Repair = 1,
        Service = 2,
    }
    public enum TicketOnBehalfs
    {
        Self = 1,
        OnBehalf = 2,
        User = 3,
        Client = 4
    }
    public enum AssetAccept
    {
        None = 0,
        Accept = 1,
        Reject = 2
    }
    public enum MyEnum { One, Two, Three }

    public enum RequestMode
    {
        Transfer = 1,
        Sharing = 2,
    }
    public enum InfoMode
    {
        Visible = 1,
        Hidden = 2,
    }
    public enum BillingMode
    {
        TimeTracker = 1,
        FixedHours = 2,
    }
    public enum ENotificationTypeCode
    {
        PRJ = 1,
        PRJASSIGN = 2,
        PRJUPDT = 3,
        TSK = 4,
        TSKASSIGN = 5,
        TASKUPDATE = 6,
        USR = 7,
        EXPAPPR = 8,
        LEAVEAPPR = 9,
        TIMEAPPR = 10,
        TICKET_CREATED = 12,
        TICKET_DELETED = 13,
        TICKET_REPLY = 14,
        TICKET_TRANSFER = 15,
        TICKET_STATUS_CHANGE = 16,

        LEAVEAPPLY = 11,
        LEAVEUPDATE = 17,
        LEAVECANCELED = 18,



        EVENT_CREATED = 19,
        EVENT_UPDATED = 20,
        EVENT_INV_ACCEPTED = 21,
        EVENT_INV_REJECTED = 22,
        EXPREJ = 23,
        LEAVEREJ = 24,
        TIMEREJ = 25,
        EXPSENTFORCORREC = 26,
        KB_ASSIGN = 27,

        LEAVEAPPLY_APPROVER = 28,
        LEAVEUPDATE_APPROVER = 29,

        KB_INFORM_CLIENT = 30,
        CB_INV_GEN = 31,
        NOTE_CREATED = 32,
        NOTE_UPDATED = 33,

        MILESTONE_UPDATED = 34,
        MILESTONE_DELETED = 35,
        MILESTONE_CREATED = 36,
        GOAL_CREATED = 37,
        GOAL_UPDATED = 38,
        REV_C_CAND = 39,

        ADDCLIENTINPROJECT = 40,
        DELETECLFROMPROJECT = 41,
        FEEDBACK_C_FOR = 42,
        FEEDBACK_C_FROM = 43,
        FEEDBACK_UPDATE_FOR = 44,
        FEEDBACK_UPDATE_FROM = 45,

        REV_C_REVIEWER = 47,
        REV_U_CAND = 48,
        REV_U_REVIEWER = 49,
        REV_D_CAND = 50,
        REV_D_REVIEWER = 51,
        NOTE_DELETED = 52,
        GOAL_DELETED = 53,
        FEEDBACK_DELETE_FOR = 54,
        FEEDBACK_DELETE_FROM = 55,

        JOB_APPLY = 56,
        JOB_ADD_APPLICANT = 57,

        CAND_F_COMMENT = 58,
        CAND_F_COMMENT_R = 59,

        REV_F_COMMENT = 60,
        REV_F_COMMENT_C = 61,

        REV_SUBMITTED_C = 62,
        REV_SUBMITTED_R = 63,
        CAND_F_COMMENT_FR = 64,
        REV_F_COMMENT_NR = 65,
        CB_INV_COMMENT = 66,
        TIMESHEETAPPR = 67,
        TIMESHEETREJ = 68,
        SCREENSHOT_COMMENT = 69,
        SCREENSHOT_FLAG = 70,
        SCREENSHOT_UNFLAG = 71,
        TIMESHEET_APPOVER = 72,
        TIMESHEET_AUTO_APPOVER = 73,
        LEAVEAPPLY_AUTO_APPROVER = 74,
        TSKADDED = 75,
        BONUS_TIMESHEET_APPROVAL = 76,
        TICKET_RESOLVE_HANDOVER = 77,
        TICKET_HOD_HANDOVER = 78,
        TICKET_HOD_RESOLVE_HANDOVER = 79,
        LEAVE_AUTO_APPROVED = 80,
        TIMESHEET_SUBMITTED = 81,
        LEAVE_APPROVAL_RECEIPT = 82,
        TIMESHEET_AUDIT_APPOVER = 83,
        TIMESHEET_SUBMIT_BY_OTHER_USER = 84,
        DELETE_GLOBAL_MESSAGE_NOTIFICATION = 85,
        TIMESHEET_AUDIT_SUBMIT_BY_OTHER_USER = 86,
        LEAVE_AUTO_APPROVED_APPROVER = 87,
        TIMESHEET_AUTO_APPROVAl_RECEIPT = 88,
        DELETE_GLOBAL_MESSAGE_COMMENT_NOTIFICATION = 89,
        PRO_REQ_CONVERTED = 90,
        PRO_REQ_ACCEPTED = 91,
        PRO_REQ_REJECTED = 92,
        PRO_REQ_CREATED = 93,
        EXP_SUBMITTED = 94,
        EXP_APPROVAL_NOTIFICATION_TO_REST_APPROVERS = 95,
        TEMPORARY_RIGHTS = 96,
        ENTRY_FLAGGED = 97,
        ENTRY_UNFLAGGED = 98,
        ENTRY_COMMENTED = 99,
        ADDCLIENTINPROJECTFORCA = 100,
        AUTO_RESPONDER = 101,
        PRO_REQ_UPDATED = 102,
        APPLICANT_HIRED = 103,
        APPLICANT_REJECTED = 104,
        APPLICANT_APPROVED = 105,
        TIME_APPROVAL_NOTIFICATION_TO_CLIENT = 106,
        TSKDELTED = 107,
        PRO_REQ_COMMENT = 108,
        TIME_ENTRY_AUTO_APPROVED = 110,
        REQUISITION_CREATED = 111,
        REQUISITION_APPROVED = 112,
        REQUISITION_REJECTED = 113,
        TRNG_CREATED = 114,
        TIME_ENTRY_SUBMIT_BY_OTHER_USER = 115,
        PROVIDE_TRNG_FEEDBACK = 116,
        CLIENT_ACTIVATED = 117,
        ASSET_ASSIGN = 118,
        ASSET_REQUISITION = 119,
        ASSET_RETURN = 120,
        ASSET_OVERDUE_ASSIGNER = 121,
        ASSET_OVERDUE_USER = 122,
        PRO_REQ_DELETED = 123,
        NEW_ASSET_REQUISITION = 124,
        LEAVE_REJECTION_NOTIFICATION_TO_REST_APPROVERS = 125,
        TIME_REJECTION_NOTIFICATION_TO_REST_APPROVERS = 126,
        EXPENSE_REJECTION_NOTIFICATION_TO_REST_APPROVERS = 127,
        ASSET_RETURN_RECIEPT = 128,
        NEW_ASSET_REQUISITION_REJECTED = 129,
        NEW_ASSET_REQUISITION_APPROVED = 130,
        NEW_ASSET_REQUISITION_COMMENT = 131,
        CLAIM_APPROVE = 132,
        CLAIM_REJECT = 133,
        CLAIM_CREATED = 134,
        TSKCLOSED = 135,
        QUOTATION_COMMENT = 136,
        QUOTATION_CREATED = 137,
        VIRTUALUSER_ASSIGNED = 138,
        QUOTATION_ACCEPTED = 139,
        QUOTATION_REJECTED = 140,
        QUOTATION_SENT = 141,
        VIRTUALUSER_REMOVED = 142,
        TRNG_TRAINER_NOTIFICATION = 143,
        WORKDAY_INVOICE_SENT = 144,
        TASKCOMPLETE = 145,
        TRNG_UPDATED = 146,
        TRNG_CANCELLED = 147,
        COURSE_SUB = 148,
        SERVICE_SUB = 149,
        CHECK_IN_OUT_MODIFIED = 150,
        CHECK_IN_OUT_REJECTED_TO_APPROVERS = 151,
        CHECK_IN_OUT_REJECTED = 152,
        CHECK_IN_OUT_APPROVED = 153,
        CHECK_IN_OUT_SUBMITTED = 154,
        TICKET_CC_OPTION = 155,
        TICKET_STATUS_CODE_ADDED = 156,
        NO_TICKET_STATUS_CODE_ADDED = 157,
        VIEW_ALL_DEPARTMENT_TICKET = 158,
        TICKET_HOD_RESOLVE_VIEW_ALL_HANDOVER = 159,
        TICKET_RESOLVE_VIEW_ALL_HANDOVER = 160,
        TICKET_HOD_VIEW_ALL_HANDOVER = 161,
        DISPUTE_RAISED = 162,
        PASSWORD_EXIPRATION_NOTIFICATION = 163,
        TICKET_MERGE = 164,
        TICKET_SPLIT = 165,
        TICKET_FEEDBACK = 166,
        TICKET_UPDATE = 167,
        TICKET_REOPEN = 168,
        TICKET_CLOSED = 169,
        TICKET_ASSIGN = 170,
        USER_SHIFT_CHANGED = 171,
        REQUEST_FOR_ASSET_RETURN = 172,
        MIN_STOCK_ALERT = 173,
        ITEM_IN_TRANSIT = 174,
        ITEM_REQUEST_QUANTITY = 175,
        ASSET_RETURN_REQUEST = 176,
        ASSET_REPLACE_REQUEST = 177,
        ASSET_RETURN_REQUEST_APPROVE = 178,
        ASSET_RETURN_REQUEST_REJECT = 179,
        ASSET_REPLACE_REQUEST_APPROVE = 180,
        ASSET_REPLACE_REQUEST_REJECT = 181,
        CHECKOUT_REQUEST = 182,
        CHECKOUT_REQUEST_REJECT = 183,

        EMP_REQUEST_CREATED = 184,
        EMP_REQUEST_APPROVED = 185,
        EMP_REQUEST_REJECTED = 186,
        EMP_REQUEST_USR_SHARED = 187,

        EXP_APPR_REIMBURSEBY_CLIENT = 188,
        EXP_APPR_COMMENT = 189,

        PRJ_TEAM_MEM_DEASSOCIATE = 190,
        PRJ_TEAM_MEM_ASSOCIATE = 191,

        CRM_DEAL_INSERT = 192,
        CRM_LEAD_INSERT = 193,
        CRM_VENDOR_INSERT = 194,
        CRM_STAGE_INSERT = 195,
        CRM_SALESORDER_INSERT = 196,
        CRM_PURCHASEORDER_INSERT = 197,
        CRM_QUOTATION_INSERT = 198,
        CRM_PRICEBOOK_INSERT = 199,
        CRM_ITEMTYPE_INSERT = 200,
        CRM_ITEM_INSERT = 201,
        CRM_INVOICE_INSERT = 202,
        CRM_FORECAST_INSERT = 203,
        CRM_CONTRACT_INSERT = 204,
        CRM_CONTACT_INSERT = 205,
        CRM_CAMPAIGN_INSERT = 206,
        CRM_ACCOUNT_INSERT = 207,
        FOLLOWUP = 208,
        CALL = 209
    }
}
