using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using RatingAdjustment.Services;
using BreadmakerReport.Models;

namespace BreadmakerReport
{
    class Program
    {
        static string dbfile = @".\data\breadmakers.db";
        static RatingAdjustmentService ratingAdjustmentService = new RatingAdjustmentService();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Bread World");
            var BreadmakerDb = new BreadMakerSqliteContext(dbfile);
            //var BMList = BreadmakerDb.Breadmakers

            // .ToList();

            var BMList = BreadmakerDb.Breadmakers.Include(rv => rv.Reviews)
                .AsEnumerable()
                .Select(bm => new {
                
                    Reviews = bm.Reviews.Count,
                    Average = Math.Round(bm.Reviews.Average(rvobj => rvobj.stars),2),
                    Adjust = Math.Round(ratingAdjustmentService.Adjust(bm.Reviews.Average(rvobj => rvobj.stars), bm.Reviews.Count()),2),
                    bm.title
                })
                .OrderByDescending(bm =>bm.Adjust)
                .ToList();

            Console.WriteLine("[#]  Reviews Average  Adjust    Description");
            for (var j = 0; j < 3; j++)
            {
                var i = BMList[j];
                // TODO: add output
                // Console.WriteLine( ... );
                Console.WriteLine("\n[{0}] {1} {2} {3} {4}", j+1,i.Reviews, i.Average, i.Adjust, i.title);

            }
        }
    }
}


