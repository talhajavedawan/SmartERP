using ERP_BL.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP_BL.Countryy
{
    public class CountryRepo
    {
        DBContextERP context = new DBContextERP();

        /// <summary>
        /// Add new country
        /// </summary>
        /// <param name="country"></param>
        public void AddCountry(Country country)
        {
            context.countries.Add(country);
            context.SaveChanges();
        }

        /// <summary>
        /// Update country
        /// </summary>
        /// <param name="country"></param>
        public void UpdateCountry(Country country)
        {
            Country countryToUpdate = context.countries.FirstOrDefault(x => x.Id == country.Id);
            countryToUpdate = country;
            context.SaveChanges();
        }

        /// <summary>
        /// Get all Countries
        /// </summary>
        /// <returns></returns>
        public List<Country> GetAllCountries()
        {
            return context.countries.ToList();
        }

        /// <summary>
        /// Get Country by Id
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public Country GetCountry(int countryId)
        {
            return context.countries.FirstOrDefault(x => x.Id == countryId);
        }


        /// <summary>
        /// Check Country Name
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public Country CheckCountryName(int countryId, string CountryName)
        {
            return context.countries.FirstOrDefault(x => x.Id != countryId && x.CountryName == CountryName);
        }

        /// <summary>
        /// Check Abbriviation
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public Country CheckAbbriviation(int countryId, string Abr)
        {
            return context.countries.FirstOrDefault(x => x.Id != countryId && x.Abbriviation == Abr);
        }

        /// <summary>
        /// Check Country Code
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public Country CheckCountryCode(int countryId, string countryCode)
        {
            return context.countries.FirstOrDefault(x => x.Id != countryId && x.CountryCode == countryCode);
        }


        /// <summary>
        /// Add new country
        /// </summary>
        /// <param name="city"></param>
        public void AddCity(City city)
        {
            context.cities.Add(city);
            context.SaveChanges();
        }

        /// <summary>
        /// Update country
        /// </summary>
        /// <param name="city"></param>
        public void UpdateCity(City city)
        {
            City countryToUpdate = context.cities.FirstOrDefault(x => x.Id == city.Id);
            countryToUpdate = city;
            context.SaveChanges();
        }

        /// <summary>
        /// Get all Countries
        /// </summary>
        /// <returns></returns>
        public List<City> GetAllCities()
        {
            return context.cities.ToList();
        }

        /// <summary>
        /// Get all Cities by Country
        /// </summary>
        /// <returns></returns>
        public List<City> GetAllCitiesByCountry(Country country)
        {
            return context.cities.Where(x=>x.countryId == country.Id).ToList();
        }

        /// <summary>
        /// Get Country by Id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public City GetCity(int cityId)
        {
            return context.cities.FirstOrDefault(x => x.Id == cityId);
        }


        /// <summary>
        /// Check Country Name
        /// </summary>
        /// <param name="country"></param>
        /// <returns></returns>
        public City CheckCityName(int cityId, string CityName)
        {
            return context.cities.FirstOrDefault(x => x.Id != cityId && x.CityName == CityName);
        }

        /// <summary>
        /// Check Abbriviation
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public City CheckCityAbbriviation(int cityId, string Abr)
        {
            return context.cities.FirstOrDefault(x => x.Id != cityId && x.Abbriviation == Abr);
        }

        /// <summary>
        /// Check Country Code
        /// </summary>
        /// <param name="PostalCode"></param>
        /// <returns></returns>
        public City CheckCityPostalCode(int CityId, string PostalCode)
        {
            return context.cities.FirstOrDefault(x => x.Id != CityId && x.PostalCode == PostalCode);
        }
    }
}
