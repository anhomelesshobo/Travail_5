using BillingManagement.Business;
using BillingManagement.Models;
using BillingManagement.UI.ViewModels.Commands;
using Inventaire;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


		private bool returnbutton = false;

		public Boolean ReturnButton
		{
			get { return returnbutton; }
			set {
				   returnbutton = value;
				   OnPropertyChanged("ReturnButton");
			    }
		}

		public Boolean ButtonEnabled 
		{
			get { return buttonenabled; }
			set
			{
				buttonenabled = value; 
				OnPropertyChanged("ButtonEnabled");
			}
		}

		BillingContext db;
		private ObservableCollection<Customer> dataCustomers;


		public ObservableCollection<Customer> DBCustomers
		{
			get => dataCustomers;
			set
			{
				dataCustomers = value;
				OnPropertyChanged();
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

		
		CustomerViewModel customerViewModel;
		InvoiceViewModel invoiceViewModel;


		public ChangeViewCommand ChangeViewCommand { get; set; }

		public DelegateCommand<object> AddNewItemCommand { get; private set; }

		public DelegateCommand<Customer> SearchCommand { get; private set; }
		public DelegateCommand<Customer> ReturnAllList { get; private set; }
		public RelayCommand CloseApplicationCommand { get; set; }
		public DelegateCommand<Invoice> DisplayInvoiceCommand { get; private set; }
		public DelegateCommand<Customer> DisplayCustomerCommand { get; private set; }

		public DelegateCommand<Customer> AddInvoiceToCustomerCommand { get; private set; }


		public MainViewModel()
		{
			db = new BillingContext();
			DBCustomers = new ObservableCollection<Customer>();
			SearchCommand = new DelegateCommand<Customer>(searchcommand);
			ReturnAllList = new DelegateCommand<Customer>(returnAllList);
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

		public void seedData(CustomerViewModel cm, InvoiceViewModel im)
		{

			if (db.Customers.Count() <= 0)
			{
				List<Customer> Customers = new CustomersDataService().GetAll().ToList();
				List<Invoice> Invoices = new InvoicesDataService(Customers).GetAll().ToList();
				foreach (Customer customer in Customers)
				{
					db.Customers.Add(customer);
				}
				db.SaveChanges();
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

		public void returnAllList(object parameter)
		{
			customerViewModel.ReturnAll();
			SearchCriteria = null;
			ReturnButton = false;
		}

		public void searchcommand(object parameter)
		{
			ReturnButton = true;
			List<Customer> customerResult = new List<Customer>();
			string input = searchCriteria as string;
			int output;
			string searchMethod;
			if (!Int32.TryParse(input, out output))
			{
				searchMethod = "name";
			}
			else
			{
				searchMethod = "id";
			}

			switch (searchMethod)
			{
				case "id":

					customerViewModel.SelectedCustomer = db.Customers.Find(output);
					break;
				case "name":
					//SelectedContact = PhoneBookBusiness.GetContactByName(input);

					customerResult = db.Customers.Where(customer => (customer.LastName.ToLower().StartsWith(input)) || (customer.Name.ToLower().StartsWith(input))).ToList();
					DBCustomers.Clear();
					if (customerResult.Count > 0)
					{

						foreach (Customer customer in customerResult)
						{

							DBCustomers.Add(customer);
						}
							
					
					}
					customerViewModel.FindMethod(DBCustomers);
					break;
				default:
					MessageBox.Show("Unkonwn search method");
					break;
			}


		}
		private void closeapplication(object parameter)
		{
			App.Current.Shutdown();
		}
	}
}
