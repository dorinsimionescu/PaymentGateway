using AutoMapper;
using Newtonsoft.Json;
using PaymentGateway.PublishedLanguage.Commands;
using PaymentGateway.PublishedLanguage.Events;
using System.Collections.Generic;

namespace PaymentGateway.Application
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            CreateMap<string, Dictionary<string, string>>()
                .ConvertUsing(x => JsonConvert.DeserializeObject<Dictionary<string, string>>(x));

            CreateMap<MakeWithdraw, WithdrawMade>();
            CreateMap<MakeNewDeposit, DepositMade>();
        }
    }
}