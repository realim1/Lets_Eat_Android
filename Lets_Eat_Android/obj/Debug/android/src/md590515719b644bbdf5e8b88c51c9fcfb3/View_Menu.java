package md590515719b644bbdf5e8b88c51c9fcfb3;


public class View_Menu
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Lets_Eat_Android.Views.Customer.View_Menu, Lets_Eat_Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", View_Menu.class, __md_methods);
	}


	public View_Menu ()
	{
		super ();
		if (getClass () == View_Menu.class)
			mono.android.TypeManager.Activate ("Lets_Eat_Android.Views.Customer.View_Menu, Lets_Eat_Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
