﻿using MvvmCross;
using Notenverwaltung.WPF.UI.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Notenverwaltung.Test
{
    [TestFixture]
    public class PrintTest : MvxTest
    {
        private IPrintService printService;

        [Test]
        public void PrintDataGridTest()
        {
            printService = Mvx.IoCProvider.Resolve<IPrintService>();

            // TODO: print on fake printer
            //MockDispatcher.RequestMainThreadAction((Action)delegate
            //{
            //    DataGrid dataGrid = new DataGrid();
            //    List<GradeModel> grades = new List<GradeModel>();
            //    grades.Add(new GradeModel() { Id = 1, Grade = 2, Abbreviation = "-", GradeEnum = Data.Enums.GradeEnum.Einzelnote });
            //    grades.Add(new GradeModel() { Id = 2, Grade = 5, Abbreviation = "+", GradeEnum = Data.Enums.GradeEnum.Einzelnote });

            //    dataGrid.ItemsSource = grades;
            //});

            // TODO: mock .ShowDialog() for testing
            //printService.PrintVisual(dataGrid, "Notenverwaltung-Test");
        }

        protected override void AdditionalSetup()
        {
            base.AdditionalSetup();

            var printService = new PrintService();
            Ioc.RegisterSingleton<IPrintService>(printService);
        }
    }
}