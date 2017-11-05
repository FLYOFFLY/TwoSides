using System;
using System.IO;

namespace TwoSides.Utils
{
    public class NamingVersion
    {
        byte _major;
        byte _minor;
        byte _build;
        byte _revesion;
        byte _idVersion, _idSubVersion;
        readonly Version _versionThis;
        public NamingVersion(byte idSubVersion, byte idVersion, byte major, byte minor, byte build, byte revesion)
        {
            // TODO: Complete member initialization
            _idSubVersion = idSubVersion;
            _idVersion = idVersion;
            _major = major;
            _minor = minor;
            _build = build;
            _revesion = revesion;
            _versionThis = new Version(major, minor, build, revesion);
        }
        public string GetVersion() => _versionThis.ToString();

        public void Load(BinaryReader reader)
        {
            _idSubVersion = reader.ReadByte();
            _idVersion = reader.ReadByte();
            _major = reader.ReadByte();
            _minor = reader.ReadByte();
            _build = reader.ReadByte();
            _revesion = reader.ReadByte();
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(_idSubVersion);
            writer.Write(_idVersion);
            writer.Write(_major);
            writer.Write(_minor);
            writer.Write(_build);
            writer.Write(_revesion);
        }
        public int GetCode() => _idSubVersion * 100000 +
                                _idVersion * 10000 +
                                _major * 1000 +
                                _minor * 100 +
                                _build * 10 +
                                _revesion;
    }
}
