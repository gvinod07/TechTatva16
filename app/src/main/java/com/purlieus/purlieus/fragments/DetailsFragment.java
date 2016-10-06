package com.purlieus.purlieus.fragments;


import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.design.widget.TextInputEditText;
import android.support.v4.app.Fragment;
import android.support.v4.content.ContextCompat;
import android.text.TextUtils;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.EditText;
import android.widget.LinearLayout;
import android.widget.NumberPicker;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.Toast;

import com.google.android.gms.common.GooglePlayServicesNotAvailableException;
import com.google.android.gms.common.GooglePlayServicesRepairableException;
import com.google.android.gms.location.places.Place;
import com.google.android.gms.location.places.ui.PlacePicker;
import com.purlieus.purlieus.R;
import com.purlieus.purlieus.activities.MapsActivity;
import com.purlieus.purlieus.application.Purlieus;

/**
 * A simple {@link Fragment} subclass.
 */
public class DetailsFragment extends Fragment{

    private int PLACE_PICKER_REQUEST = 1;
    private TextView displayedAddress;
    private double latitude;
    private double longitude;
    private EditText name;
    private EditText age;
    private EditText eMail;
    private EditText contact;
    private Spinner bgSpinner;
    private Spinner sexSpinner;
    private PlacePicker.IntentBuilder builder;

    //private static final int get_Address = 0;

    public DetailsFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_details, container, false);

        name = (EditText)view.findViewById(R.id.name_edit_text);
        age = (EditText)view.findViewById(R.id.age_edit_text);
        eMail = (EditText)view.findViewById(R.id.email_edit_text);
        contact = (EditText)view.findViewById(R.id.contact_edit_text);

        SharedPreferences sp = getActivity().getSharedPreferences(Purlieus.PROFILE_DATA, Context.MODE_PRIVATE);
        name.setText(sp.getString("name",""), TextView.BufferType.EDITABLE);

        if (sp.getInt("age",0)!=0)
            age.setText(Integer.toString(sp.getInt("age",0)), TextView.BufferType.EDITABLE);

        eMail.setText(sp.getString("email",""), TextView.BufferType.EDITABLE);
        contact.setText(sp.getString("contact", ""), TextView.BufferType.EDITABLE);

        TextView nameText = (TextView)view.findViewById(R.id.name_text_view);
        final TextView ageText = (TextView)view.findViewById(R.id.age_text_view);
        TextView emailText = (TextView)view.findViewById(R.id.email_text_view);
        TextView contactText = (TextView)view.findViewById(R.id.contact_text_view);
        displayedAddress = (TextView)view.findViewById(R.id.displayed_address_text_view);

        displayedAddress.setText(sp.getString("address",""));

        LinearLayout addressLayout = (LinearLayout)view.findViewById(R.id.select_address_layout);

        bgSpinner = (Spinner)view.findViewById(R.id.bg_spinner);
        String[] groups = {"O+", "O-", "A+", "A-", "B+", "B-", "AB+", "AB-"};
        ArrayAdapter<String> adapter = new ArrayAdapter<String>(getActivity(), android.R.layout.simple_spinner_dropdown_item, groups);
        bgSpinner.setAdapter(adapter);

        int selectBG=0;
        String bg = sp.getString("bg", "");
        for (int i=0; i<groups.length; i++){
            if (bg.equals(groups[i])){
                selectBG=i;
            }
        }
        bgSpinner.setSelection(selectBG);

        sexSpinner = (Spinner)view.findViewById(R.id.sex_spinner);
        String[] sexes = {"Male","Female","Other"};
        ArrayAdapter<String> adapter2 = new ArrayAdapter<String>(getActivity(), android.R.layout.simple_spinner_dropdown_item, sexes);
        sexSpinner.setAdapter(adapter2);

        int selectSex=0;
        String s = sp.getString("sex", "");
        for (int i=0; i<sexes.length; i++){
            if (s.equals(sexes[i])){
                selectSex=i;
            }
        }
        sexSpinner.setSelection(selectSex);

        builder = new PlacePicker.IntentBuilder();
        addressLayout.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                try {
                    startActivityForResult(builder.build(getActivity()), PLACE_PICKER_REQUEST);
                } catch (GooglePlayServicesRepairableException e) {
                    e.printStackTrace();
                } catch (GooglePlayServicesNotAvailableException e) {
                    e.printStackTrace();
                }
            }
        });
        return view;
    }

    public boolean storeDetails(){
        if (name!=null && !name.getText().toString().equals("") && age!=null && !age.getText().toString().equals("") &&eMail!=null && !eMail.getText().toString().equals("") &&  contact!=null && !contact.getText().toString().equals("") && displayedAddress!=null && !displayedAddress.getText().toString().equals("")) {
            SharedPreferences.Editor editor = getActivity().getSharedPreferences(Purlieus.PROFILE_DATA, Context.MODE_PRIVATE).edit();
            editor.putString("name", name.getText().toString());
            editor.putInt("age", Integer.parseInt(age.getText().toString()));
            editor.putString("sex", sexSpinner.getSelectedItem().toString());
            editor.putString("email", eMail.getText().toString());
            editor.putString("contact", contact.getText().toString());
            editor.putString("bg", bgSpinner.getSelectedItem().toString());
            editor.putString("address", displayedAddress.getText().toString());
            editor.putString("latitude", Double.toString(latitude));
            editor.putString("longitude", Double.toString(longitude));
            editor.apply();
            Toast.makeText(getActivity(), "Details successfully saved!", Toast.LENGTH_SHORT).show();
            return true;
        }
        return false;
    }

    public void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (requestCode == PLACE_PICKER_REQUEST) {
            if (resultCode == Activity.RESULT_OK) {
                Place place = PlacePicker.getPlace(getActivity(), data);
                if (place!=null){
                    displayedAddress.setText(place.getAddress());
                    latitude = place.getLatLng().latitude;
                    longitude = place.getLatLng().longitude;
                }
                else{
                    Log.d("Null", "intent");
                }
            }
        }
    }

}
