﻿namespace HSE.RP.API.Models.Payment.Response;


public class PaymentResponseModel
{
    public string CreatedDate { get; set; }
    public string Status { get; set; }
    public bool Finished { get; set; }
    public string PaymentLink { get; set; }
    public int Amount { get; set; }
    public string Email { get; set; }
    public string Reference { get; set; }
    public string Description { get; set; }
    public string ReturnURL { get; set; }
    public string PaymentId { get; set; }
    public string PaymentProvider { get; set; }
    public string ProviderId { get; set; }
    public string LastFourDigitsCardNumber { get; set; }
    public string FirstDigitsCardNumber { get; set; }
    public string CardExpiryDate { get; set; }
    public string CardBrand { get; set; }
    public string CardType { get; set; }
    public string AddressLineOne { get; set; }
    public string AddressLineTwo { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public string Postcode { get; set; }
    public string bsr_paymentId { get; set; }
    public string ApplicationId { get; set; }
}
