using Microsoft.EntityFrameworkCore;
using SzakemberKereso.DTOs.ResidentialAddress;
using SzakemberKereso.Models;
using System.Text.RegularExpressions;

namespace SzakemberKereso.Services
{
    public class AddressService(Context context)
    {
        //Kossuth Lajos utca 15." (streetName: "Kossuth Lajos", publicAreaType: "utca", buildingNumber: 15)
        private static (string streetName, string publicAreaType, int buildingNumber)? ParseStreetAddress(string raw)
        {
            var match = Regex.Match(raw.Trim(), @"^(.+)\s+(\S+)\s+(\d+)\.?\s*$");
            if (match.Success && int.TryParse(match.Groups[3].Value, out var num))
                return (match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim(), num);

            return null;
        }

        //finds existing address: if found, returns id; if not, creates new address and returns new id; if settlement not found or address format invalid, returns null
        public async Task<int?> ResolveAsync(InputResidentialAddressDto dto)
        {
            var settlement = await context.Settlements
                .FirstOrDefaultAsync(s =>
                    s.PostalCode == dto.Settlement.PostalCode &&
                    s.Name.ToLower() == dto.Settlement.Name.ToLower());

            if (settlement == null) return null;

            var parsed = ParseStreetAddress(dto.StreetAddress);
            if (parsed == null) return null;

            var (streetName, publicAreaType, buildingNumber) = parsed.Value;

            var existing = await context.ResidentialAddresses
                .FirstOrDefaultAsync(a =>
                    a.SettlementId == settlement.Id &&
                    a.StreetName == streetName &&
                    a.PublicAreaType == publicAreaType &&
                    a.BuildingNumber == buildingNumber);

            if (existing != null) return existing.Id;

            var address = new ResidentialAddress
            {
                SettlementId = settlement.Id,
                StreetName = streetName,
                PublicAreaType = publicAreaType,
                BuildingNumber = buildingNumber,
            };

            context.ResidentialAddresses.Add(address);
            await context.SaveChangesAsync();
            return address.Id;
        }
    }
}
