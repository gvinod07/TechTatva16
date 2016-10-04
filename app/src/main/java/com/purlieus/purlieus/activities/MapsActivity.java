package com.purlieus.purlieus.activities;


import android.Manifest;
import android.app.ActionBar;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.support.v4.app.ActivityCompat;
import android.support.v4.app.FragmentActivity;
import android.location.Location;
import android.net.Uri;
import android.os.Bundle;

import com.google.android.gms.maps.CameraUpdateFactory;
import com.google.android.gms.maps.GoogleMap;
import com.google.android.gms.maps.OnMapReadyCallback;
import com.google.android.gms.maps.SupportMapFragment;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.MarkerOptions;
import com.google.android.gms.maps.model.Polyline;
import com.google.android.gms.maps.model.PolylineOptions;
import com.purlieus.purlieus.R;

import java.util.LinkedList;
import java.util.List;

public class MapsActivity extends FragmentActivity implements OnMapReadyCallback, GoogleMap.OnMyLocationChangeListener {

    private GoogleMap mMap;
    private List<LatLng> points = new LinkedList<>();

    @Override
    public void onBackPressed() {
        Intent intent = new Intent(this, MainActivity.class);
        startActivity(intent);
        finishAffinity();
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_maps);
        // Obtain the SupportMapFragment and get notified when the map is ready to be used.
        SupportMapFragment mapFragment = (SupportMapFragment) getSupportFragmentManager()
                .findFragmentById(R.id.map);
        mapFragment.getMapAsync(this);

        final ActionBar actionBar = getActionBar();
        if (actionBar != null) {
            actionBar.setDisplayHomeAsUpEnabled(true);
        }
    }

    @Override
    public void onMapReady(GoogleMap googleMap) {
        mMap = googleMap;
        /**mMap.setMapType(GoogleMap.MAP_TYPE_NORMAL);
        mMap.setBuildingsEnabled(true);
        mMap.setIndoorEnabled(true);
        mMap.setTrafficEnabled(true);*/
        if (ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_FINE_LOCATION) != PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(this, Manifest.permission.ACCESS_COARSE_LOCATION) != PackageManager.PERMISSION_GRANTED) {
            // TODO: Consider overriding
            // public void onRequestPermissionsResult(int requestCode, String[] permissions, int[] grantResults)
            // to handle the case where the user grants the permission.

            // Permissions strings to access user's location
            String[] permissions = new String[]{
                    Manifest.permission.ACCESS_COARSE_LOCATION,
                    Manifest.permission.ACCESS_FINE_LOCATION
            };
            // Request permission in Marshmallow style
            ActivityCompat.requestPermissions(this, permissions, 1);
            return;
        } else {
            mMap.setBuildingsEnabled(true);
            mMap.setIndoorEnabled(true);
            mMap.setTrafficEnabled(true);
            mMap.setMapType(GoogleMap.MAP_TYPE_NORMAL);
            mMap.setMyLocationEnabled(true);
            mMap.setOnMyLocationChangeListener(MapsActivity.this);
            mMap.getMyLocation();
        }
        mMap.setMyLocationEnabled(true);

    }

    public void onMyLocationChange(final Location location) {
        final LatLng currentLocation = new LatLng(location.getLatitude(), location.getLongitude());

        // Add visited point to list
        points.add(currentLocation);

        // Remove all previous markers
        mMap.clear();
        // Redraw first & last marker
        mMap.addMarker(new MarkerOptions().position(currentLocation).title("You are here"));
        mMap.addMarker(new MarkerOptions().position(points.get(0)).title("You started here"));

        mMap.animateCamera(CameraUpdateFactory.newLatLngZoom(currentLocation, 16));

        final Polyline polyline = mMap.addPolyline(new PolylineOptions().add(currentLocation));
        polyline.setWidth(5);
        polyline.setColor(getResources().getColor(R.color.green));
        // Redraw polyline with all visited points
        polyline.setPoints(points);
    }
}
