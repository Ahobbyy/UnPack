using TMPro;
using UnityEngine;

namespace I2.Loc
{
	public class Localize : MonoBehaviour
	{
		private string LastLocalizedLanguage;

		public string mTerm = string.Empty;

		public string mTermSecondary = string.Empty;

		public bool LocalizeOnAwake = true;

		public Object mTarget;

		public string Term
		{
			get
			{
				return mTerm;
			}
			set
			{
				SetTerm(value);
			}
		}

		public string SecondaryTerm
		{
			get
			{
				return mTermSecondary;
			}
			set
			{
				SetTerm(null, value);
			}
		}

		public void SetTerm(string primary, string secondary = null)
		{
			if (!string.IsNullOrEmpty(primary))
			{
				mTerm = primary;
			}
			if (!string.IsNullOrEmpty(secondary))
			{
				mTermSecondary = secondary;
			}
			OnLocalize(Force: true);
		}

		private void Awake()
		{
			if (mTarget == (Object)null)
			{
				mTarget = (Object)(object)((Component)this).GetComponent<TextMeshPro>();
				if (mTarget == (Object)null)
				{
					mTarget = (Object)(object)((Component)this).GetComponent<TextMeshProUGUI>();
				}
			}
			if (LocalizeOnAwake)
			{
				OnLocalize();
			}
		}

		public void OnLocalize(bool Force = false)
		{
			if (!string.IsNullOrEmpty(mTerm) && !(mTarget == (Object)null) && (Force || (((Behaviour)this).get_enabled() && !((Object)(object)((Component)this).get_gameObject() == (Object)null) && ((Component)this).get_gameObject().get_activeInHierarchy())) && !string.IsNullOrEmpty(LocalizationManager.CurrentLanguage) && (Force || !(LastLocalizedLanguage == LocalizationManager.CurrentLanguage)))
			{
				LastLocalizedLanguage = LocalizationManager.CurrentLanguage;
				TMP_Text tMP_Text = mTarget as TextMeshProUGUI;
				if ((Object)(object)tMP_Text == (Object)null)
				{
					tMP_Text = mTarget as TextMeshPro;
				}
				string termTranslation = LocalizationManager.GetTermTranslation(mTerm);
				if (Object.op_Implicit((Object)(object)tMP_Text))
				{
					tMP_Text.text = termTranslation;
				}
				else
				{
					Debug.Log((object)("text box wrong type: " + ((Object)this).get_name() + " " + mTerm));
				}
			}
		}

		public Localize()
			: this()
		{
		}
	}
}
