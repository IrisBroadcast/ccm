using CCM.StatisticsWeb.Models;
using CCM.StatisticsWeb.Services;
using CCM.StatisticsWeb.Statistics;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CCM.StatisticsWeb.Pages
{
    public partial class CategoryStatisticsResult
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }
        [Inject]
        public IStatisticsDataService StatisticsDataService { get; set; }
        private CategoryStatisticsModel categoryStatisticsModel { get; set; } = new CategoryStatisticsModel();

        [Parameter]
        public IEnumerable<DateBasedCategoryStatistics> DateBasedCategoryStatisticsList { get; set; }
        public List<CategoryDataStatistics> CategoryDataStatisticsList { get; set; } = new List<CategoryDataStatistics>();
        public List<CategoryNumberOfCalls> catNumOfCalls { get; set; } = new List<CategoryNumberOfCalls>();
        public List<DateBasedCategoryNumberOfCalls> dateNumberOfCalls { get; set; } = new List<DateBasedCategoryNumberOfCalls>();
        public List<CategoryTotalTimeOfCalls> dateTotalTime { get; set; } = new List<CategoryTotalTimeOfCalls>();
        [Parameter]
        public IEnumerable<Region> Regions { get; set; }
        private List<string> CategoryNames { get; set; } = new List<string>();
        private List<CategoryNumberOfCalls> CategoryNumberOfCallsForRegion { get; set; } = new List<CategoryNumberOfCalls>();
        public string RegionName { get; set; } = "Alla Regioner";
        public string CategoryName { get; set; }



        //protected async override void OnInitialized()
        //{

        //}

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                GetCategoryDataFromDates();
                GetCategoryNames();
                await CreateNumberOfCallsPieChart("Alla Regioner");
                await CreateBarChartForNumberOfCalls("Alla Regioner", "MEDi");
            }
        }

        private async void CreatePieChart(ChangeEventArgs e)
        {
            RegionName = e.Value.ToString();
            await CreateNumberOfCallsPieChart(RegionName);
        }

        private async void CreateBarChart(ChangeEventArgs e)
        {
            CategoryName = e.Value.ToString();
            await CreateBarChartForNumberOfCalls(RegionName, CategoryName);
        }

        private async Task CreateNumberOfCallsPieChart(string regionName)
        {
            CategoryNumberOfCallsForRegion.Clear();
            CategoryDataStatisticsList.Clear();

            if (string.IsNullOrEmpty(regionName) || regionName == "Alla Regioner")
            {
                GetCategoryDataFromDates();
                GetNumberOfCallsStats();
            }
            else
            {
                GetCategoryNumberOfCallsForRegion(regionName);
                GetNumberOfCallsForRegion();
            }

            var obj = JsonSerializer.Serialize(catNumOfCalls);

            await JSRuntime.InvokeAsync<string>("CreatePieChart", obj);

        }

        //private async Task GetTotalTimePieChart()
        //{
        //    GetTotalTimeForCallsStats();
        //    var obj = JsonSerializer.Serialize(catTotalTime);
        //    await JSRuntime.InvokeAsync<string>("CreatePieChart", obj);
        //}

        private async Task CreateBarChartForNumberOfCalls(string regionName, string categoryName = "MEDi")
        {
            if (string.IsNullOrEmpty(regionName) || regionName == "Alla Regioner")
            {
                GetNumberOfCallsForEachDateInPeriod(categoryName);
            }
            else
            {
                GetNumberOfCallsForEachDateInPeriod(categoryName, regionName);
            }

            var obj = JsonSerializer.Serialize(dateNumberOfCalls);
            await JSRuntime.InvokeAsync<string>("CreateBarChart", obj);
        }


        private void GetCategoryDataFromDates()
        {
            foreach (var date in DateBasedCategoryStatisticsList)
            {
                foreach (var region in date.RegionCategories)
                {
                    foreach (var stat in region.CategoryStatisticsList)
                    {
                        AddCategoryDataToList(stat);
                    }
                }
            }
        }

        public void GetCategoryNames()
        {
            foreach (var date in DateBasedCategoryStatisticsList)

            {
                foreach (var reg in date.RegionCategories)
                {
                    foreach (var category in reg.CategoryStatisticsList)
                        if (!CategoryNames.Contains(category.Name))
                        {
                            CategoryNames.Add(category.Name);
                        }
                }
            }
        }

        private void GetCategoryNumberOfCallsForRegion(string regionName)
        {
            foreach (var date in DateBasedCategoryStatisticsList)
            {
                foreach (var reg in date.RegionCategories.Where(x => x.Name == regionName))
                {
                    foreach (var category in reg.CategoryStatisticsList)
                    {
                        AddCategoryForRegion(category);
                    }
                }
            }
        }

        // For a specific region: this maps to the list that converts to JSON-array for being passed to JS
        public void GetNumberOfCallsForRegion()
        {
            catNumOfCalls.Clear();
            foreach (var item in CategoryNumberOfCallsForRegion)
            {
                catNumOfCalls.Add(
                    new CategoryNumberOfCalls
                    {
                        Name = item.Name,
                        NumberOfCalls = item.NumberOfCalls
                    });
            }
        }

        // For all regions: this maps to the list that converts to JSON-array for being passed to JS
        public void GetNumberOfCallsStats()
        {
            catNumOfCalls.Clear();
            foreach (var item in CategoryDataStatisticsList)
            {
                catNumOfCalls.Add(
                new CategoryNumberOfCalls
                {
                    Name = item.Name,
                    NumberOfCalls = item.NumberOfCalls
                });
            }
        }

        public void AddCategoryForRegion(CategoryStatistics categoryStatistics)
        {
            if (!CategoryNumberOfCallsForRegion.Any(x => x.Name == categoryStatistics.Name))
            {
                CategoryNumberOfCallsForRegion.Add(new CategoryNumberOfCalls
                {
                    Name = categoryStatistics.Name,
                    NumberOfCalls = categoryStatistics.NumberOfCalls

                });
            }
            else
            {
                foreach (var item in CategoryNumberOfCallsForRegion)
                {
                    if (item.Name == categoryStatistics.Name)
                    {
                        item.NumberOfCalls += categoryStatistics.NumberOfCalls;
                    }
                }
            }
        }

        public void AddCategoryDataToList(CategoryStatistics categoryStatistics)
        {
            if (!CategoryDataStatisticsList.Any(x => x.Name == categoryStatistics.Name))
            {
                CategoryDataStatisticsList.Add(new CategoryDataStatistics
                {
                    Name = categoryStatistics.Name,
                    NumberOfCalls = categoryStatistics.NumberOfCalls,
                    TotalTimeForCalls = categoryStatistics.TotalTimeForCalls
                });
            }

            else
            {
                foreach (var item in CategoryDataStatisticsList)
                {
                    if (item.Name == categoryStatistics.Name)
                    {
                        item.NumberOfCalls += categoryStatistics.NumberOfCalls;
                        item.TotalTimeForCalls += categoryStatistics.TotalTimeForCalls;
                    }
                }
            }
        }

        //public void GetTotalTimeForCallsStats()
        //{
        //    catTotalTime.Clear();

        //    foreach (var item in CategoryDataStatisticsList)
        //    {
        //        catTotalTime.Add(new CategoryTotalTimeOfCalls
        //        {
        //            Name = item.Name,
        //            TotalTimeForCalls = Math.Round((Double)item.TotalTimeForCalls, 2)
        //        });
        //    }
        //}


        public void GetNumberOfCallsForEachDateInPeriod(string categoryName, string regionName)
        {
            dateNumberOfCalls.Clear();

            foreach (var date in DateBasedCategoryStatisticsList)
            {
                if (date.RegionCategories.Any(x => x.Name == regionName))
                    foreach (var region in date.RegionCategories.Where(x => x.Name == regionName))
                    {
                        foreach (var item in region.CategoryStatisticsList)
                        {
                            if (item.Name == categoryName)
                                dateNumberOfCalls.Add(new DateBasedCategoryNumberOfCalls
                                {
                                    Date = date.Date.ToShortDateString(),
                                    NumberOfCalls = item.NumberOfCalls

                                });
                        }
                    }
                else
                {
                    dateNumberOfCalls.Add(new DateBasedCategoryNumberOfCalls
                    {
                        Date = date.Date.ToShortDateString(),
                        NumberOfCalls = 0
                    });
                }
            }
        }

        public void GetNumberOfCallsForEachDateInPeriod(string categoryName)
        {
            dateNumberOfCalls.Clear();

            foreach (var date in DateBasedCategoryStatisticsList)
            {
                if (date.RegionCategories.Any())
                {
                    foreach (var region in date.RegionCategories)
                    {
                        foreach (var item in region.CategoryStatisticsList)
                        {
                            if (item.Name == categoryName)
                                dateNumberOfCalls.Add(new DateBasedCategoryNumberOfCalls
                                {
                                    Date = date.Date.ToShortDateString(),
                                    NumberOfCalls = item.NumberOfCalls

                                });
                        }
                    }
                }
                else
                {
                    dateNumberOfCalls.Add(new DateBasedCategoryNumberOfCalls
                    {
                        Date = date.Date.ToShortDateString(),
                        NumberOfCalls = 0
                    });
                }
            }
        }
    }

    public class CategoryDataStatistics
    {
        public string Name { get; set; }
        public int NumberOfCalls { get; set; }
        public double TotalTimeForCalls { get; set; }
    }
    public class CategoryNumberOfCalls
    {
        public string Name { get; set; }
        public int NumberOfCalls { get; set; }
    }

    public class CategoryTotalTimeOfCalls
    {
        public DateTime Date { get; set; }
        public double TotalTimeForCalls { get; set; }
    }

    public class DateBasedCategoryNumberOfCalls
    {
        public string Date { get; set; }
        public int NumberOfCalls { get; set; }
    }
}
