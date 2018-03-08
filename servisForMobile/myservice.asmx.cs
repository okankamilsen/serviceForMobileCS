using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace servisForMobile
{
    /// <summary>
    /// Summary description for myservice
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]

    public class myservice : System.Web.Services.WebService
    {


        [WebMethod]
        public bool setMenu(Int32 id, String name)
        {

            profileDataContext myContext = new profileDataContext();

            menu m = new menu();
            m.menuID = id;//float.Parse(latitude, System.Globalization.CultureInfo.InvariantCulture);
            m.menuName = name;//float.Parse(longitude, System.Globalization.CultureInfo.InvariantCulture);


            myContext.menus.InsertOnSubmit(m);
            myContext.SubmitChanges();
            return true;

        }
        //menu name alır
        [WebMethod]
        public List<string> getMenuName()
        {
            List<string> a = new List<string>();
            a = null;
            profileDataContext myContext = new profileDataContext();
            // CURRENT_LOCATION getLocation = new CURRENT_LOCATION();

            var menuName = from i in myContext.menus
                                  select i.menuName;
            // getLocation =  locationContext;
            if (menuName.ToList() != null)
            {
                a = menuName.ToList();
            }
            return a;
        }
        //table name alır
        [WebMethod]
        public List<string> getTableName()
        {
            List<string> a = new List<string>();
            a = null;
            profileDataContext myContext = new profileDataContext();
            // CURRENT_LOCATION getLocation = new CURRENT_LOCATION();

            var tableName = from i in myContext.restaurantTables
                                  select i.tableName;
            // getLocation =  locationContext;
            if (tableName.ToList() != null)
            {
                a = tableName.ToList();

            }
            return a;
        }
        //menu içerik alır
        [WebMethod]
        public List<menuItem> getMenuItemList(int menuID)
        {
            List<menuItem> a = new List<menuItem>();
            a = null;
            profileDataContext myContext = new profileDataContext();
            // CURRENT_LOCATION getLocation = new CURRENT_LOCATION();

            var menuItemList = from i in myContext.menuItems
                            where i.menuID.Equals(menuID)
                            select i;
            // getLocation =  locationContext;
            if (menuItemList.ToList() != null)
            {
                a = menuItemList.ToList();
            }
            return a;
        }
        //ödenmemiş masa siparişini gösterir
        // input table id
        // output siparişler adetleri fiyatları
        [WebMethod]
        public List<orderTablePrice> getTableOrder(int tableID)
        {
            List<orderTablePrice> a = new List<orderTablePrice>();
            a = null;
            profileDataContext myContext = new profileDataContext();

            var menuItemList = from toi in myContext.tableOrderItems
                               from mi in myContext.menuItems
                               from tto in myContext.tableOrders
                               where tto.tableID.Equals(tableID)
                               where toi.menuItemID.Equals(mi.menuItemID)
                               where tto.isPaid.Equals("False")
                               where tto.tableOrderID.Equals(toi.tableOrderID)
                               select new orderTablePrice { itemName=mi.itemName ,
                                   quantity= toi.quantity.Value,
                                   totalPrice= mi.price.Value * toi.quantity.Value};
            if (menuItemList.ToList() != null)
            {
                a = menuItemList.ToList();
            }
            return a;
        }
        //hesap al 
        //input masa id
        [WebMethod]
        public bool takeReceipt(int tableID)
        {
            profileDataContext myContext = new profileDataContext();

            tableOrder tOrder = new tableOrder();
            
            if (getTableOrder(tableID) != null)
            {
                // Query the database for the row to be updated.
                var query =
                    from tord in myContext.tableOrders
                    where tord.tableID == tableID
                    select tord;

                // Execute the query, and change the column values
                // you want to change.
                foreach (tableOrder tord in query)
                {
                    tord.isPaid = true;
                    // Insert any additional changes to column values.
                }

                // Submit the changes to the database.
                try
                {
                    myContext.SubmitChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                    return false;
                }
            }
            return false;
        }
        //garson çağır 
        //input masa id
        [WebMethod]
        public bool callWaiters(int tableID)
        {
            profileDataContext myContext = new profileDataContext();

            restaurantTable rTable = new restaurantTable();
            
                // Query the database for the row to be updated.
                var query =
                    from tord in myContext.restaurantTables
                    where tord.tableID == tableID
                    select tord;

                // Execute the query, and change the column values
                // you want to change.
                foreach (restaurantTable tord in query)
                {
                    tord.callWaiter = true;
                    // Insert any additional changes to column values.
                }

                // Submit the changes to the database.
                try
                {
                    myContext.SubmitChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    // Provide for exceptions.
                    return false;
                }
            return false;
        }
        //masaya garson gönder
        //input masa id
        [WebMethod]
        public bool responseWaiters(int tableID)
        {
            profileDataContext myContext = new profileDataContext();

            restaurantTable rTable = new restaurantTable();

            // Query the database for the row to be updated.
            var query =
                from tord in myContext.restaurantTables
                where tord.tableID == tableID
                select tord;

            // Execute the query, and change the column values
            // you want to change.
            foreach (restaurantTable tord in query)
            {
                tord.callWaiter = false;
                // Insert any additional changes to column values.
            }

            // Submit the changes to the database.
            try
            {
                myContext.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Provide for exceptions.
                return false;
            }
            return false;
        }
    }
}

