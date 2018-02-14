using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TicketPurchase.Services
{
    public class Paypal_IPN
    {
        HttpRequest _request;
        string _txnID, _txnType, _paymentStatus, _receiverEmail, _itemName, _itemNumber, _quantity, _invoice, _custom,
        _paymentGross, _payerEmail, _pendingReason, _paymentDate, _paymentFee, _firstName, _lastName, _address,
        _city, _state, _zip, _country, _countryCode, _addressStatus, _payerStatus, _payerID, _paymentType, _notifyVersion,
        _verifySign, _response, _payerPhone, _payerBusinessName, _business, _receiverID, _memo, _tax, _qtyCartItems,
        _shippingMethod, _shipping;
        private string _postUrl = "";
        private string _strRequest = "";
        private string _smtpHost, _fromEmail, _toEmail, _fromEmailPassword, _smtpPort;
        /// <summary>
        /// valid strings are "TEST" for sandbox use 
        /// "LIVE" for production use
        /// "ELITE" for test use off of PayPal...avoid having to be logged into PayPal Developer
        /// </summary>
        /// <param name="mode"></param>
        public Paypal_IPN(string mode, HttpRequest request)
        {
            this._request = request;
            if (mode.ToLower() == "test")
                this.PostUrl = "https://ipnpb.sandbox.paypal.com/cgi-bin/webscr";
            else if (mode.ToLower() == "live")
                this.PostUrl = "https://www.paypal.com/cgi-bin/webscr";
            else if (mode.ToLower() == "elite")
                this.PostUrl = "http://www.eliteweaver.co.uk/testing/ipntest.php";
            else
                this.PostUrl = "";
            this.fillProperties();
        }
        public string PostUrl
        {
            get { return _postUrl; }
            set { _postUrl = value; }
        }
        /// <summary>
        /// This is the reponse back from the http post back to PayPal.
        /// Possible values are "VERIFIED" or "INVALID"
        /// </summary>
        public string Response
        {
            get { return _response; }
        }
        public string RequestLength
        {
            get { return _strRequest; }
        }
        public string SmtpHost
        {
            get { return _smtpHost; }
        }
        public string SmtpPort
        {
            get { return _smtpPort; }
        }
        public string FromEmail
        {
            get { return _fromEmail; }
        }
        public string FromEmailPassword
        {
            get { return _fromEmailPassword; }
        }
        public string ToEmail
        {
            get { return _toEmail; }
        }
        /// <summary>
        /// Email address or Account ID of the payment recipient.  This is equivalent
        ///  to the value of receiver_email if the payment is sent to the primary account
        /// , which is most cases it is.  This value is that value of what is set in the button html
        /// markup.  This value also get normalized to lowercase when coming back from PayPal
        /// </summary>
        public string Business
        {
            get { return _business; }
            set { _business = value; }
        }
        /// <summary>
        /// Unique Paypal transaction ID
        /// </summary>
        public string TXN_ID { get { return _txnID; } }
        /// <summary>
        /// Type of transaction from the customer. Possible values are
        /// "cart", "express_checkout", "send_money", "virtual_terminal", "web-accept"
        /// </summary>
        public string TXN_Type { get { return _txnType; } }
        /// <summary>
        /// This is the status of the payment from the Customer.Possible values are: 
        /// "Canceled_Reversal", "Completed", "Denied", "Expired", "Failed", "Pending",
        ///  "Processed", "Refunded", "Reversed", "Voided"
        /// </summary>
        public string PaymentStatus { get { return _paymentStatus; } }
        public string ReceiverEmail { get { return _receiverEmail; } }
        public string ReceiverID { get { return _receiverID; } }
        public string ItemName { get { return _itemName; } }
        public string ItemNumber { get { return _itemNumber; } }
        public string Quantity { get { return _quantity; } }
        public string QuantityCartItems { get { return _qtyCartItems; } }
        public string Invoice { get { return _invoice; } }
        public string Custom { get { return _custom; } }
        public string Memo { get { return _memo; } }
        public string Tax { get { return _tax; } }
        public string PaymentGross { get { return _paymentGross; } }
        public string PaymentDate { get { return _paymentDate; } }
        public string PaymentFee { get { return _paymentFee; } }
        public string PayerEmail { get { return _payerEmail; } }
        public string PayerPhone { get { return _payerPhone; } }
        public string PayerBusinessName { get { return _payerBusinessName; } }
        public string PendingReason { get { return _pendingReason; } }
        public string ShippingMethod { get { return _shippingMethod; } }
        public string Shipping { get { return _shipping; } }
        public string PayerFirstName { get { return _firstName; } }
        public string PayerLastName { get { return _lastName; } }
        public string PayerAddress { get { return _address; } }
        public string PayerCity { get { return _city; } }
        public string PayerState { get { return _state; } }
        public string PayerZipCode { get { return _zip; } }
        public string PayerCountry { get { return _country; } }
        public string PayerCountryCode { get { return _countryCode; } }
        public string PayerAddressStatus { get { return _addressStatus; } }
        /// <summary>
        /// Customer either had a verified or unverified account with PayPal. 
        /// Possible return values from PayPal are "verified" or "unverified"
        /// </summary>
        public string PayerStatus { get { return _payerStatus; } }
        public string PayerID { get { return _payerID; } }
        public string PaymentType { get { return _paymentType; } }
        /// <summary>
        /// This is the version number of the IPN that makes the post.
        /// </summary>
        public string NotifyVersion { get { return _notifyVersion; } }
        /// <summary>
        /// An encrypted string that is used to validate the transaction. You don't have to use this for anything
        ///  unless you want to keep it and store it for your records.
        /// </summary>
        public string VerifySign { get { return _verifySign; } }
        private void fillProperties()
        {
            this._strRequest = _request.Form.ToString();
            this._city = _request.Form["address_city"];
            this._country = _request.Form["address_country"];
            this._countryCode = _request.Form["address_country_code"];
            this._state = _request.Form["address_state"];
            this._addressStatus = _request.Form["address_status"];
            this._address = _request.Form["address_street"];
            this._zip = _request.Form["address_zip"];
            this._firstName = _request.Form["first_name"];
            this._lastName = _request.Form["last_name"];
            this._payerBusinessName = _request.Form["payer_business_name"];
            this._payerEmail = _request.Form["payer_email"];
            this._payerID = _request.Form["payer_id"];
            this._payerStatus = _request.Form["payer_status"];
            this._payerPhone = _request.Form["contact_phone"];
            this._business = _request.Form["business"];
            this._itemName = _request.Form["item_name"];
            this._itemNumber = _request.Form["item_number"];
            this._quantity = _request.Form["quantity"];
            this._receiverEmail = _request.Form["receiver_email"];
            this._receiverID = _request.Form["receiver_id"];
            this._custom = _request.Form["custom"];
            this._memo = _request.Form["memo"];
            this._invoice = _request.Form["invoice"];
            this._tax = _request.Form["tax"];
            this._qtyCartItems = _request.Form["num_cart_items"];
            this._paymentDate = _request.Form["payment_date"];
            this._paymentStatus = _request.Form["payment_status"];
            this._paymentType = _request.Form["payment_type"];
            this._pendingReason = _request.Form["pending_reason"];
            this._txnID = _request.Form["txn_id"];
            this._txnType = _request.Form["txn_type"];
            this._paymentFee = _request.Form["mc_fee"];
            this._paymentGross = _request.Form["mc_gross"];
            this._notifyVersion = _request.Form["notify_version"];
            this._verifySign = _request.Form["verify_sign"];
        }
    }

}
