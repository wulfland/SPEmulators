using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using SharePointSample.Solution.Core;

namespace SalesOrderPortalSolution.WebParts
{
    [ToolboxItemAttribute(false)]
    public class SalesOrderWebPart : WebPart, ISalesOrderView
    {
        private SalesOrderPresenter presenter;
        public DropDownList CustomersDropDownList;
        public GridView SalesOrderGrid;

        public SalesOrderWebPart()
        {
            this.presenter = new SalesOrderPresenter(this, new CustomerListRepository(), new SalesOrderListRepository());
        }

        public void Inject(ICustomerRepository customerRepository, ISalesOrderRepository salesOrderRepository)
        {
            this.presenter = new SalesOrderPresenter(this, customerRepository, salesOrderRepository);
        }

        protected override void CreateChildControls()
        {
            this.CustomersDropDownList = new DropDownList();
            this.CustomersDropDownList.SelectedIndexChanged += new EventHandler(OnCustomersDropDownListSelectedIndexChanged);
            this.CustomersDropDownList.DataTextField = "Name";
            this.CustomersDropDownList.DataValueField = "CustomerId";
            this.CustomersDropDownList.AutoPostBack = true;
            this.Controls.Add(this.CustomersDropDownList);

            this.SalesOrderGrid = new GridView();
            this.Controls.Add(SalesOrderGrid);
        }

        public void OnCustomersDropDownListSelectedIndexChanged(object sender, EventArgs e)
        {
            presenter.SelectCustomer(int.Parse(this.CustomersDropDownList.SelectedValue));
        }


        protected override void OnLoad(EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                EnsureChildControls();
                presenter.Initialize();
            }
        }

        public IEnumerable<Customer> Customers
        {
            get
            {
                return this.CustomersDropDownList.DataSource as IEnumerable<Customer>;
            }
            set
            {
                this.CustomersDropDownList.DataSource = value;
                this.CustomersDropDownList.DataBind();
            }
        }

        public IEnumerable<SalesOrder> SalesOrders
        {
            get
            {
                return this.SalesOrderGrid.DataSource as IEnumerable<SalesOrder>;
            }
            set
            {
                this.SalesOrderGrid.DataSource = value;
                this.SalesOrderGrid.DataBind();
            }
        }
    }
}
