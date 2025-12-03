using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Threading;

// e.g. usage rsListbox.Sort(ref ListBoxToSort, rsListbox.SortOrder.Descending);


[Serializable()]  
public sealed class rsListbox
{
    public enum SortOrder
    {
        Ascending, Descending
    };

    private class Reverser :  IComparer
    {
        // You can use CaseInsensitiveComparer.Compare if you want Non-casesensitive comparison
        int System.Collections.IComparer.Compare(Object x, Object y)
        {
            int iCompare = 0;
            iCompare=( (new Comparer(Thread.CurrentThread.CurrentCulture)).Compare( y, x ) );

            return iCompare;
        }

    }

    public static void Sort(ref System.Web.UI.WebControls.ListBox myListBox, SortOrder mySortOrder)
    {
        try
        {

        

        SortedList myList;
        IComparer myComparer = new Reverser();

        if (mySortOrder == SortOrder.Ascending)
            myList = new SortedList();
        else
            myList = new SortedList(myComparer);

        for (int i = 0; i < myListBox.Items.Count; i++)
            myList.Add(myListBox.Items[i].Text.Trim(),
            myListBox.Items[i].Value.Trim());

        myListBox.Items.Clear();

        for (int i = 0; i < myList.Count; i++)
            myListBox.Items.Add(new
            System.Web.UI.WebControls.ListItem(myList.GetKey(i).ToString().Trim(),
            myList.GetByIndex(i).ToString().Trim()));
        
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
    }
    public static void Sort(ref System.Web.UI.WebControls.DropDownList myListBox, SortOrder mySortOrder)
    {
        try
        {
            SortedList myList;
            IComparer myComparer = new Reverser();

            if (mySortOrder == SortOrder.Ascending)
                myList = new SortedList();
            else
                myList = new SortedList(myComparer);

            for (int i = 0; i < myListBox.Items.Count; i++)
                myList.Add(myListBox.Items[i].Text.Trim(),
                myListBox.Items[i].Value.Trim());

            myListBox.Items.Clear();

            for (int i = 0; i < myList.Count; i++)
                myListBox.Items.Add(new
                System.Web.UI.WebControls.ListItem(myList.GetKey(i).ToString().Trim(),
                myList.GetByIndex(i).ToString().Trim()));

        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine(ex.ToString());
        }
    }
}

