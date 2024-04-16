namespace net.shonx.privatenotes.backend;

using Newtonsoft.Json;

public class Token
{
    public class Header
    {
        public string typ { get; set; }
        public string nonce { get; set; }
        public string alg { get; set; }
        public string x5t { get; set; }
        public string kid { get; set; }

        public Header(string typ, string nonce, string alg, string x5t, string kid)
        {
            this.typ = typ;
            this.nonce = nonce;
            this.alg = alg;
            this.x5t = x5t;
            this.kid = kid;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Payload
    {
        public string aud { get; set; }
        public string iss { get; set; }
        public int iat { get; set; }
        public int nbf { get; set; }
        public int exp { get; set; }
        public int acct { get; set; }
        public string acr { get; set; }
        public string aio { get; set; }
        public string altsecid { get; set; }
        public string[] amr { get; set; }
        public string app_displayname { get; set; }
        public string appid { get; set; }
        public string appidacr { get; set; }
        public string email { get; set; }
        public string family_name { get; set; }
        public string given_name { get; set; }
        public string idp { get; set; }
        public string idtyp { get; set; }
        public string ipaddr { get; set; }
        public string name { get; set; }
        public string oid { get; set; }
        public string platf { get; set; }
        public string puid { get; set; }
        public string rh { get; set; }
        public string scp { get; set; }
        public string[] signin_state { get; set; }
        public string sub { get; set; }
        public string tenant_region_scope { get; set; }
        public string tid { get; set; }
        public string unique_name { get; set; }
        public string uti { get; set; }
        public string ver { get; set; }
        public string[] wids { get; set; }
        public Dictionary<string, string> xms_st { get; set; }
        public int xms_tcdt { get; set; }

        public Payload(string aud, string iss, int iat, int nbf, int exp, int acct, string acr, string aio, string altsecid, string[] amr, string app_displayname, string appid, string appidacr, string email, string family_name, string given_name, string idp, string idtyp, string ipaddr, string name, string oid, string platf, string puid, string rh, string scp, string[] signin_state, string sub, string tenant_region_scope, string tid, string unique_name, string uti, string ver, string[] wids, Dictionary<string, string> xms_st, int xms_tcdt)
        {
            this.aud = aud;
            this.iss = iss;
            this.iat = iat;
            this.nbf = nbf;
            this.exp = exp;
            this.acct = acct;
            this.acr = acr;
            this.aio = aio;
            this.altsecid = altsecid;
            this.amr = amr;
            this.app_displayname = app_displayname;
            this.appid = appid;
            this.appidacr = appidacr;
            this.email = email;
            this.family_name = family_name;
            this.given_name = given_name;
            this.idp = idp;
            this.idtyp = idtyp;
            this.ipaddr = ipaddr;
            this.name = name;
            this.oid = oid;
            this.platf = platf;
            this.puid = puid;
            this.rh = rh;
            this.scp = scp;
            this.signin_state = signin_state;
            this.sub = sub;
            this.tenant_region_scope = tenant_region_scope;
            this.tid = tid;
            this.unique_name = unique_name;
            this.uti = uti;
            this.ver = ver;
            this.wids = wids;
            this.xms_st = xms_st;
            this.xms_tcdt = xms_tcdt;
        }

        public bool isExpired()
        {
            int currentTime = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            return currentTime >= exp;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}