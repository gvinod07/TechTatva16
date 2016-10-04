package com.purlieus.purlieus.fragments;


import android.content.Intent;
import android.os.Bundle;
import android.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.purlieus.purlieus.R;
import com.purlieus.purlieus.activities.MapsActivity;

/**
 * A simple {@link Fragment} subclass.
 */
public class DetailsFragment extends Fragment {

    //private static final int get_Address = 0;

    public DetailsFragment() {
        // Required empty public constructor
    }


    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {
        // Inflate the layout for this fragment
        View view = inflater.inflate(R.layout.fragment_details, container, false);

        TextView addressText = (TextView) view.findViewById(R.id.Address);
        addressText.setOnClickListener(new View.OnClickListener(){

            @Override
            public void onClick(View v){
                Intent intent = new Intent(getActivity(), MapsActivity.class);
                startActivity(intent);
            }
        });
        return view;
    }


}
