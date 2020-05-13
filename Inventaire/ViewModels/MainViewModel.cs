using BillingManagement.Models;
using BillingManagement.UI.ViewModels.Commands;
using Inventaire;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BillingManagement.UI.ViewModels
{
    class MainViewModel : BaseViewModel
    {
		private BaseViewModel _vm;
		private bool buttonenabled = true;


		public Boolean ButtonEnabled 
		{
			get { return buttonenabled; }
			set
			{
				buttonenabled = value; 
				OnPropertyChanged("ButtonEnabled"); // 
			}
		}
		public BaseViewModel VM
		{
			get { return _vm; }
			set {
				_vm = value;
				OnPropertyChanged();
			}
		}

		private string searchCriteria;

		public string SearchCriteria
		{
			get { return searchCriteria; }
			set { 
				searchCriteria = value;
				OnPropertyChanged();
			}
		}

		BillingContext db = new BillingContext();
		CustomerViewModel customerViewModel;
		InvoiceViewModel invoiceViewModel;

		public ChangeViewCommand ChangeViewCommand { get; set; }

		public DelegateCommand<object> AddNewItemCommand { get; private set; }

		public DelegateCommand<Customer> SearchCommand { get; private set; }
		public RelayCommand CloseApplicationCommand { get; set; }
		public DelegateCommand<Invoice> DisplayInvoiceCommand { get; private set; }
		public DelegateCommand<Customer> DisplayCustomerCommand { get; private set; }

		public DelegateCommand<Customer> AddInvoiceToCustomerCommand { get; private set; }


		public MainViewModel()
		{
			SearchCommand = new DelegateCommand<Customer>(searchcommand);
			ChangeViewCommand = new ChangeViewCommand(ChangeView);
			CloseApplicationCommand = new RelayCommand(closeapplication);
			DisplayInvoiceCommand = new DelegateCommand<Invoice>(DisplayInvoice);
			DisplayCustomerCommand = new DelegateCommand<Customer>(DisplayCustomer);

			AddNewItemCommand = new DelegateCommand<object>(AddNewItem, CanAddNewItem);
			AddInvoiceToCustomerCommand = new DelegateCommand<Customer>(AddInvoiceToCustomer);

			customerViewModel = new CustomerViewModel();
			invoiceViewModel = new InvoiceViewModel(customerViewModel.Customers);
			seedData(customerViewModel, invoiceViewModel);
			VM = customerViewModel;

		}

		void seedData(CustomerViewModel cm, InvoiceViewModel im)
		{
			if (db.Customers != null)
			{
				db = new BillingContext();

			}
			else
			{
				for (int i = 0; i < cm.Customers.Count; i++)
				{
					db.Customers.Add(new Customer()
					{
						Name = cm.Customers[i].Name,
						Address = cm.Customers[i].Address,
						City = cm.Customers[i].City,
						PostalCode = cm.Customers[i].PostalCode,
						LastName = cm.Customers[i].LastName,
						Province = cm.Customers[i].Province,
						CustomerID = cm.Customers[i].CustomerID,
						ContactInfos = cm.Customers[i].ContactInfos,
						PicturePath = cm.Customers[i].PicturePath
					});

					db.Invoices.Add(new Invoice()
					{
						SubTotal = im.Invoices[i].SubTotal,
						Customer = im.Invoices[i].Customer,

					});


					db.SaveChanges();

				}
			


			}





		}


		private void ChangeView(string vm)
		{
			switch (vm)
			{
				case "customers":
					ButtonEnabled = true;
					VM = customerViewModel;					
					break;
				case "invoices":
					ButtonEnabled = false;
					VM = invoiceViewModel;
					break;
			}
		}

		private void DisplayInvoice(Invoice invoice)
		{
			invoiceViewModel.SelectedInvoice = invoice;
			VM = invoiceViewModel;
		}

		private void DisplayCustomer(Customer customer)
		{
			customerViewModel.SelectedCustomer = customer;
			VM = customerViewModel;
		}

		private void AddInvoiceToCustomer(Customer c)
		{
			var invoice = new Invoice(c);
			c.Invoices.Add(invoice);
			DisplayInvoice(invoice);
		}

		private void AddNewItem (object item)
		{
			if (VM == customerViewModel)
			{
				var c = new Customer();
				customerViewModel.Customers.Add(c);
				customerViewModel.SelectedCustomer = c;
			}
		}

		private bool CanAddNewItem(object o)
		{
			bool result = false;

			result = VM == customerViewModel;
			return result;
		}

		private void searchcommand(object parameter)
		{
			
		
			


		}
		private void closeapplication(object parameter)
		{
			App.Current.Shutdown();
		}
	}
}
