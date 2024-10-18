﻿using Microsoft.EntityFrameworkCore;
using WebApplication7.Models;
using WebApplication7.Repositry.IRepositry;
using WebApplication7.ViewModels;
namespace WebApplication7.Repositry
{
    public class Place_Repo : IPlace
    {
        DepiContext _context;
        public Place_Repo(DepiContext _context)
        {
            this._context = _context;
        }
        public PlaceViewModel GetAll()
        {
            var ans= new PlaceViewModel();
            ans.RelatedPlaces = _context.Places.ToList();
            return ans;
        }
        public PlaceViewModel GetAllMuseum()
        {
            var ans=new PlaceViewModel();
            ans.RelatedPlaces=_context.Places.Where(x=>x.Place_Type=="Museum").ToList();
            return ans;
        } public PlaceViewModel GetAllHotels()
        {
            var ans=new PlaceViewModel();
            ans.RelatedPlaces=_context.Places.Where(x=>x.Place_Type=="Hotel").ToList();
            return ans;
        }
        public Place GetById(int id)
        {
            return _context.Places.FirstOrDefault(x => x.Place_Id == id);
        }


        public PlaceViewModel Get(int id)
        {
            var relatedPlaces = _context.Places.Where(p => p.Place_Id != id).Take(4).ToList();
            var specificPlace = _context.Places.FirstOrDefault(x => x.Place_Id == id);
            var viewModel = new PlaceViewModel
            {

                SpecificPlace = specificPlace,
                RelatedPlaces = relatedPlaces
            };
            return viewModel;
        }

        public void Add(Place place)
        {
            _context.Places.Add(place);
             Save();
        }

        public void Edit(PlaceViewModel place)
        {
            var existingPlace = _context.Places.Find(place.SpecificPlace.Place_Id);
            if (existingPlace != null)
            {
                existingPlace.Place_Name = place.SpecificPlace.Place_Name;
                existingPlace.Place_Type = place.SpecificPlace.Place_Type;
                existingPlace.Place_City = place.SpecificPlace.Place_City;
                existingPlace.Place_Price = place.SpecificPlace.Place_Price;
                existingPlace.Place_Rating = place.SpecificPlace.Place_Rating;
                existingPlace.Description = place.SpecificPlace.Description;
                if (place.SpecificPlace.clientFile != null)
                {
                    MemoryStream stream = new MemoryStream();
                    place.SpecificPlace.clientFile.CopyTo(stream);
                    existingPlace.dbimage = stream.ToArray();
                }
                else
                {
                    place.SpecificPlace.dbimage = existingPlace.dbimage;
                }
                Save();
            }
        }

        public void Delete(int id)
        {
            var place = _context.Places.Find(id);
            if (place != null)
            {
                _context.Places.Remove(place);
                Save();
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
