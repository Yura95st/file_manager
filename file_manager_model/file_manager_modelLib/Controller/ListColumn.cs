using System;
using System.IO;
using System.Collections.Generic;


class ListColumn
{
    private List<FileSystemElement> element_list_;
    private int active_element_;
    private int[] selected_elements_;
    private string path_;
    private List<MyDrive> drives_;
    private int active_drive_;

    public string Path
    {
        get
        {
            return path_;
        }
        set
        {
            //TODO: Check valid value
            path_ = value;
        }
    }

    public int ActiveElement
    {
        get
        {
            return active_element_;
        }
        set
        {
            //TODO: Check valid value
            active_element_ = value;
        }
    }

    public int ActiveDrive_
    {
        get
        {
            return active_drive_;
        }
        set
        {
            //TODO: Check valid value
            active_drive_ = value;
        }
    }

    public List<MyDrive> Drives
    {
        get
        {
            return drives_;
        }
    }

    public void Select()
    {

    }

    public void BuildDrives()
    {
        DriveInfo[] allDrives = DriveInfo.GetDrives();

        foreach (DriveInfo d in allDrives)
        {
            MyDrive my_drive = new MyDrive(d.Name);

            try
            {
                my_drive.Label = d.VolumeLabel;
                my_drive.Type = d.DriveType;
                my_drive.Format = d.DriveFormat;
                my_drive.FreeSpace = d.AvailableFreeSpace;
                my_drive.Size = d.TotalSize;

                drives_.Add(my_drive);
            }
            catch (Exception e)
            {

            }
            finally
            { }
        }
    }

    public void BuildElements()
    {
        this.BuildDirectories();
        this.BuildFiles();
    }

    private void BuildFiles()
    {
        DirectoryInfo di = new DirectoryInfo(@path_);
        try
        {
            foreach (FileInfo f in di.GetFiles())
            {
                MyFile my_file = new MyFile(f.Name);

                try
                {
                    my_file.Extension = f.Extension;
                    my_file.Size = f.Length;
                    my_file.Parent = f.Directory;
                    my_file.CreationDate = f.CreationTimeUtc;

                    element_list_.Add(my_file);
                }
                catch (Exception e)
                {

                }
                finally
                { }
            }
        }
        catch (Exception e)
        {

        }
        finally { }
    }

    private void BuildDirectories()
    {
        DirectoryInfo di = new DirectoryInfo(@path_);
        try
        {
            foreach (DirectoryInfo d in di.GetDirectories())
            {
                MyDirectory my_directory = new MyDirectory(d.Name);

                try
                {
                    my_directory.Parent = d.Parent;
                    my_directory.CreationDate = d.CreationTimeUtc;

                    element_list_.Add(my_directory);
                }
                catch (Exception e)
                {

                }
                finally
                { }
            }
        }
        catch (Exception e)
        {

        }
        finally { }
    }
}

