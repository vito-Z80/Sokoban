namespace Objects
{
    public class ContactorBoxContainer : MainObject
    {
        bool m_contacted;

        public bool GetContact() => m_contacted;
        
        
        public void SubmitContact()
        {
            m_contacted = true;
        }

        public void BreakContact()
        {
            m_contacted = false;
        }
    }
}