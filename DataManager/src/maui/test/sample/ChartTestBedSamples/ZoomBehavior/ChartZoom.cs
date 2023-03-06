using System;
using System.Collections;
using System.Collections.Generic;
using ChartTestBedSamples.Axis;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Syncfusion.Maui.Charts;

namespace ChartTestBedSamples
{
	public partial class ChartZoom : ContentPage
	{
        ChartAxis selectedAxis; 
		public ChartZoom()
		{
			InitializeComponent();
            axis_picker.SelectedIndexChanged += Axis_picker_SelectedIndexChanged;
            factor_slider.ValueChanged += Factor_slider_ValueChanged;
            pos_slider.ValueChanged += Pos_slider_ValueChanged;
            Z_in.Clicked += Z_in_Clicked;
            Z_out.Clicked += Z_out_Clicked;
            Z_Reset.Clicked += Z_Reset_Clicked;
            Z_ToFactor.Clicked += Z_ToFactor_Clicked;
            E_zoom.Toggled += E_zoom_Toggled;
            E_Pann.Toggled += E_Pann_Toggled;
            E_DTap.Toggled += E_DTap_Toggled;
            mood_picker.SelectedIndexChanged += Mood_picker_SelectedIndexChanged;
        }

        private void Mood_picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = (sender as Picker).SelectedIndex;
            zoomBehavior.ZoomMode = index == 0 ? ZoomMode.XY : index == 1 ? ZoomMode.X : ZoomMode.Y;

        }

        private void E_DTap_Toggled(object sender, ToggledEventArgs e)
        {
            header_label.Text = "Statue: " + " Enable Double tap Zooming";
            zoomBehavior.EnableDoubleTap = !zoomBehavior.EnableDoubleTap;
        }

        private void E_Pann_Toggled(object sender, ToggledEventArgs e)
        {
            header_label.Text = "Statue: " + " Enable Pan Zooming";
            zoomBehavior.EnablePanning = !zoomBehavior.EnablePanning;
        }

        private void E_zoom_Toggled(object sender, ToggledEventArgs e)
        {
            header_label.Text = "Statue: " + " Enable Pinch Zooming";
            zoomBehavior.EnablePinchZooming = !zoomBehavior.EnablePinchZooming;
        }

        private void Z_ToFactor_Clicked(object sender, EventArgs e)
        {
            zoomBehavior.ZoomToFactor(selectedAxis, 0.2, 0.25);
        }

        private void Z_Reset_Clicked(object sender, EventArgs e)
        {
            header_label.Text = "Statue: " + "Invoke Zoom Reset";
            zoomBehavior.Reset(Chart);
        }

        private void Z_out_Clicked(object sender, EventArgs e)
        {
            header_label.Text = "Statue: " + "Invoke Zoom Out";
            zoomBehavior.ZoomOut(Chart);
        }

        private void Z_in_Clicked(object sender, EventArgs e)
        {
            header_label.Text = "Statue: " + "Invoke Zoom In";
            zoomBehavior.ZoomIn(Chart);
        }

        private void Pos_slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var value = (double)e.NewValue;
            selectedAxis.ZoomPosition = value;
        }

        private void Factor_slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var value  = (double)e.NewValue;
            selectedAxis.ZoomFactor = value;
        }

        private void Axis_picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var index = (sender as Picker).SelectedIndex;

            selectedAxis = index == 0 ? p_axis : s_axis;
        }
    }
}
