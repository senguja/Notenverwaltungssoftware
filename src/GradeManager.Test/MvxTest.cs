﻿using MvvmCross.Base;
using MvvmCross.Tests;
using MvvmCross.Views;
using NUnit.Framework;

namespace GradeManager.Test
{
    public class MvxTest : MvxIoCSupportingTest
    {
        protected MockMvxViewDispatcher MockDispatcher
        {
            get;
            private set;
        }

        [SetUp]
        public virtual void SetupTest()
        {
            Setup();

            // By default, ExcelDataReader throws a NotSupportedException "No data is
            // available for encoding 1252." on .NET Core.
            // https://github.com/ExcelDataReader/ExcelDataReader/blob/develop/README.md#asdataset-configuration-options
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        protected override void AdditionalSetup()
        {
            base.AdditionalSetup();

            MockDispatcher = new MockMvxViewDispatcher();
            Ioc.RegisterSingleton<IMvxMainThreadDispatcher>(MockDispatcher);
            Ioc.RegisterSingleton<IMvxViewDispatcher>(MockDispatcher);
        }
    }
}