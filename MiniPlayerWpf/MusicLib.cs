using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;


namespace MiniPlayerWpf
{
    class MusicLib
    {
        private DataSet musicDataSet;
        // The constructor reads the music.xsd and music.xml files into the DataSet
        public MusicLib()
        // Adds a song to the music library and returns the song's ID. The song
        // parameter's ID is also set to the auto-generated ID.
        {
            musicDataSet = new DataSet();
            musicDataSet.ReadXmlSchema("music.xsd");
            musicDataSet.ReadXml("music.xml");
        }
        public EnumerableRowCollection<string> SongIds
        {
            get
            {
                var ids = from row in musicDataSet.Tables["song"].AsEnumerable()
                          orderby row["id"]
                          select row["id"].ToString();
                return ids;
            }
        }
       public void PrintAllTables()
        {
            foreach (DataTable table in musicDataSet.Tables)
            {
                Console.WriteLine("Table name = " + table.TableName);
                foreach (DataRow row in table.Rows)
                {
                    Console.WriteLine("Row:");
                    int i = 0;
                    foreach (Object item in row.ItemArray)
                    {
                        Console.WriteLine(" " + table.Columns[i].Caption + "=" + item);
                        i++;
                    }
                }
                Console.WriteLine();
            }
        }
        public int AddSong(Song song)
        // Return a Song for the given song ID or null if no song was not found.
        {
            DataTable table = musicDataSet.Tables["song"];
            DataRow row = table.NewRow();
            row["title"] = song.Title;
            row["artist"] = song.Artist;
            row["album"] = song.Album;
            row["filename"] = song.Filename;
            row["length"] = song.Length;
            row["genre"] = song.Genre;
            table.Rows.Add(row);
            // Get the song's newly assigned ID
            song.Id = Convert.ToInt32(row["id"]);
            return song.Id;
        }
        public Song GetSong(int songId)
        // Update the given song with the given song ID. Returns true if the song
        // was updated, false if it could not because the song ID was not found.
        {
        }
        public bool UpdateSong(int songId, Song song)
    // Delete a song given the song's ID. Return true if the song was
    // successfully deleted, false if the song ID was not found.
    {
        DataTable table = musicDataSet.Tables["song"];

        // Only one row should be selected
        foreach (DataRow row in table.Select("id=" + songId))
        {
                row["title"] = song.Title;  //titleTextBox.Text;
                row["artist"] = song.Artist; // artistTextBox.Text;
                row["album"] = song.Album;//albumTextBox.Text;
                row["genre"] = song.Genre; //genreTextBox.Text;
                row["length"] = song.Length;//lengthTextBox.Text;
                row["filename"] = song.Filename; //filenameTextBox.Text;

                song.Id = Convert.ToInt32(row["id"]);
                return true;
        }
            return false;
    }
        public bool DeleteSong(int songId)
        {
            bool isDeleted = false;

            DataTable table = musicDataSet.Tables["song"];
            table.Rows.Remove(table.Rows.Find(songId));

            List<DataRow> rows = new List<DataRow>();
            table = musicDataSet.Tables["playlist_song"];
            foreach (DataRow row in table.Rows)
                if (row["song_id"].ToString() == songId.ToString())
                    rows.Add(row);

            foreach (DataRow row in rows)
            {
                row.Delete();
                isDeleted = true;
            }



            return isDeleted;

        }
        // Save the song database to the music.xml file
        public void Save()
        {
            string filename = "music.xml";
            musicDataSet.WriteXml(filename);
        }
    }
}
