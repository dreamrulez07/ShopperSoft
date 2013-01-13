﻿using Microsoft.Phone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ShopperSoft
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded +=new RoutedEventHandler(MainPage_Loaded);
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

		private void AddNewItemToItemGrid(String itemLabel, int itemId) {
			var newGrid = new Grid();
            newGrid.Tag = itemId.ToString();
            newGrid.Height = 70;
            newGrid.VerticalAlignment = VerticalAlignment.Top;
            newGrid.Width = 480;

            var linearGradient = new LinearGradientBrush();
            linearGradient.EndPoint = new Point(0.5, 1);
            linearGradient.StartPoint = new Point(0.5, 0);

            var gradientStop = new GradientStop();
            gradientStop.Color = Colors.Black;
            gradientStop.Offset = 0;
            linearGradient.GradientStops.Add(gradientStop);

            gradientStop = new GradientStop();
            var color = new Color();
            color.A = 255;
            color.R = 38;
            color.G = 38;
            color.B = 38;
            gradientStop.Color = color;
            gradientStop.Offset = 1;
            linearGradient.GradientStops.Add(gradientStop);

            newGrid.Background = linearGradient;

            var textBlock = new TextBlock();
            textBlock.Text = itemLabel;
            textBlock.HorizontalAlignment = HorizontalAlignment.Left;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Width = 310;
            textBlock.FontSize = 32;
            textBlock.Margin = new Thickness(0);
            textBlock.Padding = new Thickness(15, 9, 0, 0);

            var button = new Button();
            button.Tag = itemId.ToString();
            button.Margin = new Thickness(315, 0, 85, 0);
            button.BorderThickness = new Thickness(0);
            button.BorderBrush = null;
            button.Foreground = null;
            button.Tap += ShareItemWithFriends;

            var imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(@"\Assets\Icons\appbar.cloud.upload.png", UriKind.Relative));
            imageBrush.Stretch = Stretch.None;

            button.Background = imageBrush;

            var button2 = new Button();
            button2.Tag = itemId.ToString();
            button2.Margin = new Thickness(400, 0, 0, 0);
            button2.BorderThickness = new Thickness(0);
            button2.BorderBrush = null;
            button2.Foreground = null;
            button2.Tap += DeleteItem;

            var imageBrush2 = new ImageBrush();
            imageBrush2.ImageSource = new BitmapImage(new Uri(@"\Assets\Icons\appbar.delete.png", UriKind.Relative));
            imageBrush2.Stretch = Stretch.None;

            button2.Background = imageBrush2;

            newGrid.Children.Add(textBlock);
            newGrid.Children.Add(button);
            newGrid.Children.Add(button2);

            ItemListBox.Items.Add(newGrid);
		}

        private void AddFriendInformation(String friendName, int friendId, Dictionary<int, String> itemList)
        {
            /*
                <TextBlock TextWrapping="Wrap" Text="Friend Name 1" Height="70" Foreground="White" Padding="20,10,0,0" FontSize="32">
                </TextBlock>
                <StackPanel Height="552" ScrollViewer.VerticalScrollBarVisibility="Auto">
                    <TextBlock TextWrapping="Wrap" Text="Item 1" Height="48" FontSize="24" Padding="35,5,0,0"/>
                </StackPanel>
            */

            var textBlock = new TextBlock();
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Text = friendName;
            textBlock.Tag = friendId.ToString();
            textBlock.Height = 70;
            textBlock.Padding = new Thickness(20, 10, 0, 0);
            textBlock.FontSize = 32;
            textBlock.ManipulationStarted += ToggleFriendItemList;
            
            FriendsListPanel.Children.Add(textBlock);

            var stackPanel = new StackPanel();
            stackPanel.Visibility = Visibility.Collapsed;
            stackPanel.Tag = friendId.ToString();

            foreach (KeyValuePair <int, String> items in itemList) {
                textBlock = new TextBlock();
                textBlock.TextWrapping = TextWrapping.Wrap;
                textBlock.Height = 48;
                textBlock.FontSize = 24;
                textBlock.Padding = new Thickness(35, 5, 0, 0);
                textBlock.Text = items.Value;
                textBlock.Tag = items.Key;

                stackPanel.Children.Add(textBlock);
            }

            FriendsListPanel.Children.Add(stackPanel);
        }

        private void ToggleFriendItemList(object sender, System.Windows.Input.ManipulationStartedEventArgs e)
        {
            var friendId = ((TextBlock)sender).Tag;

            foreach (var childNode in FriendsListPanel.Children)
            {
                if (childNode is StackPanel)
                {
                    var checkId = ((StackPanel)childNode).Tag;
                    if (checkId.ToString() == friendId.ToString())
                    {
                        if (((StackPanel)childNode).Visibility == Visibility.Collapsed)
                        {
                            ((StackPanel)childNode).Visibility = Visibility.Visible;
                        }
                        else
                        {
                            ((StackPanel)childNode).Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
        }

        private void AddNewItem(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var itemName = NewItemTextBox.Text;
            NewItemTextBox.Text = "";

            // TODO: Add item to database.
            int itemId = 0; // REMOVE THIS

            /*
             * <insert item to database>
             * var itemId = <GetFromDatabase>
             */

            AddNewItemToItemGrid(itemName, itemId);
        }

        private void ShareItemWithFriends(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var itemId = ((Button)sender).Tag.ToString();

        	// TODO: Use item id to set the share flag to true in the database
        }

        private void DeleteItem(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var itemId = ((Button)sender).Tag.ToString();

        	// TODO: Use item id to remove the item form database
        }

        private void FillFriendsInformation(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
        	// TODO: Add event handler implementation here.
            if (((Pivot)sender).SelectedIndex == 1)
            {
                FriendsListPanel.Children.Clear();

                var itemList = new Dictionary<int, String>();
                itemList.Add(1, "Item 1");
                itemList.Add(2, "Item 2");
                itemList.Add(3, "Item 3");
                itemList.Add(4, "Item 4");
                itemList.Add(5, "Item 5");

                AddFriendInformation("Akshet", 1, itemList);
                AddFriendInformation("Akshet2", 2, itemList);
                AddFriendInformation("Akshet3", 3, itemList);
                AddFriendInformation("Akshet4", 4, itemList);
                AddFriendInformation("Akshet5", 5, itemList);
            }

        }
    }
}