package com.purlieus.purlieus.fragments;

import android.content.Context;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import com.purlieus.purlieus.R;
import com.purlieus.purlieus.application.Purlieus;

/**
 * Created by anurag on 6/10/16.
 */
public class DisplayDetailsFragment extends Fragment {

    private TextView name;
    private TextView age;
    private TextView sex;
    private TextView email;
    private TextView contact;
    private TextView bg;
    TextView address;

    public DisplayDetailsFragment() {
    }

    @Nullable
    @Override
    public View onCreateView(LayoutInflater inflater, @Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
        View view = inflater.inflate(R.layout.fragment_display_details, container, false);

        name = (TextView)view.findViewById(R.id.display_details_name_text_view);
        age = (TextView)view.findViewById(R.id.details_display_age_text_view);
        sex = (TextView)view.findViewById(R.id.details_display_sex_text_view);
        email = (TextView)view.findViewById(R.id.details_display_email_text_view);
        contact = (TextView)view.findViewById(R.id.details_display_phone_text_view);
        bg = (TextView)view.findViewById(R.id.display_details_bg_text_view);
        address = (TextView)view.findViewById(R.id.display_details_address_text_view);

        setData();
        return view;
    }

    public void setData(){
        SharedPreferences sp = getActivity().getSharedPreferences(Purlieus.PROFILE_DATA, Context.MODE_PRIVATE);
        if (!sp.getString("name", "").isEmpty()){
            name.setText(sp.getString("name",""));
        }
        if (sp.getInt("age",0)!=0){
            age.setText(Integer.toString(sp.getInt("age",0)));
        }
        if (!sp.getString("sex", "").isEmpty()){
            sex.setText(sp.getString("sex",""));
        }
        if (!sp.getString("email", "").isEmpty()){
            email.setText(sp.getString("email",""));
        }
        if (!sp.getString("contact", "").isEmpty()){
            contact.setText(sp.getString("contact",""));
        }
        if (!sp.getString("bg", "").isEmpty()){
            bg.setText(sp.getString("bg",""));
        }
        if (!sp.getString("address", "").isEmpty()){
            address.setText(sp.getString("address",""));
        }
    }
}
