using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TwoSides.Utils
{
    public class NamingVersion
    {
        byte major, minor, build, revesion;
        byte idVersion, idSubVersion;
        Version versionThis;
        public NamingVersion(byte idSubVersion, byte idVersion, byte major, byte minor, byte build, byte revesion)
        {
            // TODO: Complete member initialization
            this.idSubVersion = idSubVersion;
            this.idVersion = idVersion;
            this.major = major;
            this.minor = minor;
            this.build = build;
            this.revesion = revesion;
            versionThis = new Version(major, minor, build, revesion);
        }
        public string getVersion()
        {
            return versionThis.ToString();
        }
        public void load(BinaryReader reader)
        {
            idSubVersion = reader.ReadByte();
            idVersion = reader.ReadByte();
            major = reader.ReadByte();
            minor = reader.ReadByte();
            build = reader.ReadByte();
            revesion = reader.ReadByte();
        }
        public void save(BinaryWriter writer)
        {
            writer.Write(idSubVersion);
            writer.Write(idVersion);
            writer.Write(major);
            writer.Write(minor);
            writer.Write(build);
            writer.Write(revesion);
        }
        public int getCode()
        {
            return idSubVersion * 100000 +
                idVersion * 10000 +
                major * 1000 +
                minor * 100 +
                build * 10 +
                revesion;
        }
    }
}
