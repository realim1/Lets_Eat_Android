package md57cca9a4d8f644e237ec9664455fff161;


public class Ownerpage
	extends android.support.v7.app.AppCompatActivity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Lets_Eat_Android.Views.Owner.Ownerpage, Lets_Eat_Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", Ownerpage.class, __md_methods);
	}


	public Ownerpage ()
	{
		super ();
		if (getClass () == Ownerpage.class)
			mono.android.TypeManager.Activate ("Lets_Eat_Android.Views.Owner.Ownerpage, Lets_Eat_Android, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
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
